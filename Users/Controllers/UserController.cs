namespace Users.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RepositoryAPI.Models;
    using RepositoryAPI;
    using Users.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using UsersAPI.DTOs;

    //[Authorize] // Apply here to protect all endpoints in this controller

    //[ApiController]
    //[Route("api/users")]
    //public class UserController : ControllerBase
    //{
    //    private readonly IUsersService _useresService;
    //    private readonly IConfiguration _config;
    //    public UserController(IUsersService useresService, IConfiguration configuration)
    //    //public UserController(IUsersService useresService)
    //    {
    //        _useresService = useresService;
    //        _config = configuration;
    //    }
    //    [HttpPost("verify")]
    //    public async Task<IActionResult> VerifyUser([FromBody] LoginRequest request)
    //    {
    //        var user = await _useresService.GetUserByUsername(request.Username);
    //        if (user == null || user.Password != request.Password) // Replace with password hashing check
    //        {
    //            return Unauthorized();
    //        }

    //        var token = GenerateJwtToken(user);
    //        return Ok(new { Token = token });
    //    }

    //    private string GenerateJwtToken(User user)
    //    {
    //        var jwtSettings = _config.GetSection("Jwt");
    //        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

    //        var claims = new[]
    //        {
    //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") // Add role claim
    //};

    //        var token = new JwtSecurityToken(
    //            issuer: jwtSettings["Issuer"],
    //            audience: jwtSettings["Audience"],
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
    //            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }

    //    // Get: api/users
    //    [HttpGet]
    //    public async Task<IEnumerable<User>> GetUsers()
    //    {
    //        List<User> users = await _useresService.GetUsers();
    //        return users;
    //    }

    //    // Get: api/users/{id}
    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetUser(int id)
    //    {
    //        var user = await _useresService.GetUserById(id);
    //        if (user == null)
    //        {
    //            return NotFound();
    //        }

    //        var userResponse = new UserResponse
    //        {
    //            UserId = user.UserId,
    //            FirstName = user.FirstName,
    //            LastName = user.LastName,
    //            UserName = user.UserName,
    //            Email = user.Email,
    //            Phone = user.Phone,
    //            Address = user.Address,
    //            IsAdmin = user.IsAdmin
    //        };

    //        return Ok(userResponse);
    //    }

    //    // POST: api/users
    //    [HttpPost]
    //    public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        var user = new User
    //        {
    //            FirstName = userRequest.FirstName,
    //            LastName = userRequest.LastName,
    //            UserName = userRequest.UserName,
    //            Password = userRequest.Password, // Hash the password before saving
    //            Email = userRequest.Email,
    //            Phone = userRequest.Phone,
    //            Address = userRequest.Address
    //        };

    //        var result = await _useresService.AddUser(user);
    //        if (!result.Success)
    //        {
    //            return BadRequest(result.Message);
    //        }

    //        var userResponse = new UserResponse
    //        {
    //            UserId = user.UserId,
    //            FirstName = user.FirstName,
    //            LastName = user.LastName,
    //            UserName = user.UserName,
    //            Email = user.Email,
    //            Phone = user.Phone,
    //            Address = user.Address,
    //            IsAdmin = user.IsAdmin
    //        };

    //        return Ok(userResponse);
    //    }
    //    // PUT: api/users/{id}
    //    [HttpPut("{id}")]
    //    public async Task<bool> UpdateUser(int id, [FromBody] User user)
    //    {
    //        return await this._useresService.UpdateUser(id, user);
    //    }
    //    // Delete a user
    //    [HttpDelete("{id}")]
    //    [Authorize(Policy = "AdminOnly")] // Only admins can delete users
    //    public async Task<bool> DeleteUser(int id)
    //    {
    //        return await _useresService.DeleteUser(id);
    //    }

    //}
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _useresService;
        private readonly IConfiguration _config;

        public UserController(IUsersService useresService, IConfiguration configuration)
        {
            _useresService = useresService;
            _config = configuration;
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] LoginRequest request)
        {
            var user = await _useresService.GetUserByUsername(request.Username);
            if (user == null || user.Password != request.Password) // Replace with password hashing check
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var claims = new[]
            {
       new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Ensure this is set correctly
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") // Add role claim
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        // Public endpoint: Anyone can add a user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                UserName = userRequest.UserName,
                Password = userRequest.Password, // Hash the password before saving
                Email = userRequest.Email,
                Phone = userRequest.Phone,
                Address = userRequest.Address
            };

            var result = await _useresService.AddUser(user);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

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

            return Ok(userResponse);
        }

        // Only admins can get all users
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _useresService.GetUsers();
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

            return Ok(userResponses);
        }

        // Only admins can get a user by ID
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _useresService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

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

            return Ok(userResponse);
        }

        // Only admins can delete a user
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _useresService.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok("User deleted successfully.");
        }

        // Admins can modify any user, and authenticated users can modify their own information
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest userRequest)
        {
            var currentUser = await _useresService.GetUserByUsername(User.Identity.Name); // Get the currently authenticated user
            var userToUpdate = await _useresService.GetUserById(id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            // Check if the current user is an admin or the owner of the account
            if (!User.IsInRole("Admin") && currentUser.UserId != userToUpdate.UserId)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }

            // Update user information
            userToUpdate.FirstName = userRequest.FirstName;
            userToUpdate.LastName = userRequest.LastName;
            userToUpdate.UserName = userRequest.UserName;
            userToUpdate.Password = userRequest.Password; // Hash the password before saving
            userToUpdate.Email = userRequest.Email;
            userToUpdate.Phone = userRequest.Phone;
            userToUpdate.Address = userRequest.Address;

            var result = await _useresService.UpdateUser(id, userToUpdate);
            if (!result)
            {
                return BadRequest("Failed to update user.");
            }

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

            return Ok(userResponse);
        }
        /*
         *  [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest userRequest)
        {
            // Ensure the user is authenticated
            if (User.Identity?.Name == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Get the currently authenticated user
            var currentUser = await _useresService.GetUserByUsername(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound("Current user not found.");
            }

            // Get the user to update
            var userToUpdate = await _useresService.GetUserById(id);
            if (userToUpdate == null)
            {
                return NotFound("User to update not found.");
            }

            // Check if the current user is an admin or the owner of the account
            if (!User.IsInRole("Admin") && currentUser.UserId != userToUpdate.UserId)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }

            // Update user information
            userToUpdate.FirstName = userRequest.FirstName;
            userToUpdate.LastName = userRequest.LastName;
            userToUpdate.UserName = userRequest.UserName;
            userToUpdate.Password = userRequest.Password; // Hash the password before saving
            userToUpdate.Email = userRequest.Email;
            userToUpdate.Phone = userRequest.Phone;
            userToUpdate.Address = userRequest.Address;

            var result = await _useresService.UpdateUser(id, userToUpdate);
            if (!result)
            {
                return BadRequest("Failed to update user.");
            }

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

            return Ok(userResponse);
        }
        */
    }
}
