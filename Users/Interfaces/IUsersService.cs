using RepositoryAPI.Models;
namespace Users.Interfaces
{
    public interface IUsersService
    {
        Task<List<User>> GetUsers();
        Task<User> GetUserById(int id);
        public Task<(bool Success, string Message)> AddUser(User user);
        Task<bool> UpdateUser(int id, User user);
        Task<bool> DeleteUser(int id);
        Task<User> GetUserByUsername(string username);
    }
}
