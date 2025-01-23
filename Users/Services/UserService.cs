using Microsoft.EntityFrameworkCore;
using RepositoryAPI;
using RepositoryAPI.Models;
using Users.Interfaces;

namespace Users.Services
{
    public class UserService : IUsersService

    {
        private readonly EcommerceDbContext _context;

        public UserService(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<(bool Success, string Message)> AddUser(User user)
        {
            //if (user == null) return (false, "User data is required.");

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == user.UserName);

            if (existingUser != null)
            {
                return (false, "Username already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return (true, "User added successfully.");
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

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            //return Task.FromResult(new List<User>());
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public Task<bool> UpdateUser(int id, User user)
        {
            var dbuser = _context.Users.Find(id);
            if (dbuser == null)
            {
                return Task.FromResult(false);
            }
            dbuser.FirstName = user.FirstName;
            dbuser.LastName = user.LastName;
            dbuser.Email = user.Email;
            dbuser.Phone = user.Phone;
            dbuser.Password = user.Password;
            dbuser.Address = user.Address;
            _context.SaveChanges();

            return Task.FromResult(true);
        }
        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
