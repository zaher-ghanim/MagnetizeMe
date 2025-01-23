using AuthSerrviceAPI.Interface;
using AuthService.Helpers;
using Microsoft.EntityFrameworkCore;
using RepositoryAPI;

namespace AuthSerrviceAPI.Services
{
    public class AuthService: IAuthService
    {
        private readonly EcommerceDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthService(EcommerceDbContext context, JwtTokenHelper jwtTokenHelper)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null || user.Password != password) // In real-world apps, use password hashing
            {
                return null;
            }

            return _jwtTokenHelper.GenerateToken(user.UserName, user.IsAdmin);
        }
    }
}
