using RepositoryAPI.Models;
using System.Security.Claims;
using UsersAPI.DTOs;
namespace Users.Interfaces
{
    public interface IUsersService
    {
        public  Task<List<UserResponse>> GetUsers();
        public Task<UserResponse> GetUserById(int id);
        public Task<(bool Success, string Message, UserResponse UserResponse)> AddUser(UserRequest userRequest);
        public Task<(bool Success, string Message, UserResponse UserResponse)> UpdateUser(int id, UserRequest userRequest, ClaimsPrincipal currentUser);
        Task<bool> DeleteUser(int id);
        Task<User> GetUserByUsername(string username);
        public string VerifyUser(string username, string password);
    }
}
