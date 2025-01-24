using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryAPI;
using RepositoryAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Users.Interfaces;
using UsersAPI.DTOs;

namespace Users.Services
{
    public class UserService : IUsersService

    {
        private readonly EcommerceDbContext _context;
        private readonly IConfiguration _config;

        public UserService(EcommerceDbContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }
        public async Task<(bool Success, string Message, UserResponse UserResponse)> AddUser(UserRequest userRequest)
        {
            // Check if the userRequest object is null
            if (userRequest == null)
            {
                return (false, "User data is required.", null);
            }

            // Check if the username already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userRequest.UserName);

            if (existingUser != null)
            {
                return (false, "Username already exists.", null);
            }

            // Create the User object from the UserRequest
            var user = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                UserName = userRequest.UserName,
                Password = HashPassword(userRequest.Password), // Hash the password
                Email = userRequest.Email,
                Phone = userRequest.Phone,
                Address = userRequest.Address,
                CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow), // Set the CreatedAt date
                //IsAdmin = userRequest.IsAdmin // Optional: Set IsAdmin if provided in the request
            };

            // Add the user to the context and save changes
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Create the UserResponse object
            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                IsAdmin = user.IsAdmin
            };

            return (true, "User added successfully.", userResponse);
        }

        private string HashPassword(string password)
        {
            using (SHA512 myHash = SHA512.Create())
            {
                byte[] hash = myHash.ComputeHash(Encoding.UTF8.GetBytes(password));
                string x = Convert.ToBase64String(hash);
                return Convert.ToBase64String(hash);
            }
        }


        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return await Task.FromResult(false);
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null; // User not found
            }

            // Map the User entity to a UserResponse object
            var userResponse = new UserResponse
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                IsAdmin = user.IsAdmin
            };

            return userResponse;
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Map the list of User entities to a list of UserResponse objects
            var userResponses = users.Select(u => new UserResponse
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Phone = u.Phone,
                Address = u.Address,
                IsAdmin = u.IsAdmin
            }).ToList();

            return userResponses;
        }

        public async Task<(bool Success, string Message, UserResponse UserResponse)> UpdateUser(int id, UserRequest userRequest, ClaimsPrincipal currentUser)
        {
           
            var currentUsername = currentUser.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

            // Fetch the currently authenticated user
            var currentUserEntity = await GetUserByUsername(currentUsername);
            if (currentUserEntity == null)
            {
                return (false, "Current user not found.", null);
            }

            // Fetch the user to update
            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null)
            {
                return (false, "User not found.", null);
            }

            // Check if the current user is an admin or the owner of the account
            if (!currentUser.IsInRole("Admin") && currentUserEntity.UserId != userToUpdate.UserId)
            {
                return (false, "You are not authorized to update this user.", null);
            }

            // Update user information
            userToUpdate.FirstName = userRequest.FirstName;
            userToUpdate.LastName = userRequest.LastName;
            userToUpdate.UserName = userRequest.UserName;
            userToUpdate.Email = userRequest.Email;
            userToUpdate.Password=HashPassword(userToUpdate.Password);
            userToUpdate.Phone = userRequest.Phone;
            userToUpdate.Address = userRequest.Address;

            // Hash the password if it's being updated
            if (!string.IsNullOrEmpty(userRequest.Password))
            {
                userToUpdate.Password = HashPassword(userRequest.Password);
            }

            try
            {
                await _context.SaveChangesAsync();

                // Create the UserResponse object
                var userResponse = new UserResponse
                {
                    UserId = userToUpdate.UserId,
                    FirstName = userToUpdate.FirstName,
                    LastName = userToUpdate.LastName,
                    UserName = userToUpdate.UserName,
                    Email = userToUpdate.Email,
                    Phone = userToUpdate.Phone,
                    Address = userToUpdate.Address,
                    IsAdmin = userToUpdate.IsAdmin
                };

                return (true, "User updated successfully.", userResponse);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return (false, "An error occurred while updating the user.", null);
            }
        }
        public string VerifyUser(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user is null || (user.Password != HashPassword(password)))
            {
                return null;
            }

            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Name, user.UserName), // Ensure this is set to the username
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
        new Claim("UserId", user.UserId.ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

    }
}
