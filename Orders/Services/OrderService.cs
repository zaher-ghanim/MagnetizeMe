using Microsoft.EntityFrameworkCore;
using RepositoryAPI;
using RepositoryAPI.Models;
using RepositoryAPI.Models.OrdersDTO;
using Users.Interfaces;

namespace OrdersAPI.Services
{
    public class OrderService : IOrdersService
    {
        private readonly EcommerceDbContext _context;

        public OrderService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<(int orderid,bool Success, string Message)> CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return (order.OrderId,true, "Order successfully completed.");
        }
       

        public async Task<bool> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return await Task.FromResult(false);
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return await Task.FromResult(true);
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }
            return order;
        }
       

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }
       
        public async Task<List<Order>> GetOrdersByStatus(bool status)
        {
            var orders = await _context.Orders.Where(x => x.Status == status).ToListAsync();
            return orders;
        }


        public async Task<List<Order>> GetOrdersByUsername(string username)
        {
            int? userID = _context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync().Result.UserId;
            if (userID is null)
                return null;
            var orders = await _context.Orders.Where(x => x.UserId == userID).ToListAsync();
            return orders;
        }



        //must be used in a Helper Class
        public async Task<string> GetUsernameFromUserId(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return user?.UserName;
        }
        public async Task<Size> GetOrdersSize(int size_id)
        {
            Size size = await _context.Sizes.FindAsync(size_id);
            if (size == null)
            {
                return null;
            }
            return size;
        }
        public async Task<bool> UpdateOrderStatus(int orderId, bool status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return false; // Order not found
            }

            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true; // Status updated successfully
        }

        public async Task<bool> GetOrderStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found."); // Or return a default value
            }

            return order.Status;
        }
    }
}
