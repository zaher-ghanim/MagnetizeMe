using RepositoryAPI.Models;
using RepositoryAPI.Models.OrdersDTO;

namespace Users.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Order>> GetOrders();
        public Task<Size> GetOrdersSize(int sizeID);

        Task<Order> GetOrderById(int id);
        public Task<(int orderid, bool Success, string Message)> CreateOrder(Order user);
        public Task<string> GetUsernameFromUserId(int userId);
        Task<bool> DeleteOrder(int id);

        //+Jess > To be implemented later if we have time
        Task<List<Order>> GetOrdersByUsername(string username);
        Task<List<Order>> GetOrdersByStatus(bool status);
        Task<bool> UpdateOrderStatus(int orderId, bool status);
        Task<bool> GetOrderStatus(int orderId);
    }
}
