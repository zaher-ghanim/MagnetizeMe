using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryAPI.Models;
using RepositoryAPI.Models.OrdersDTO;
using System.Security.Claims;
using Users.Interfaces;

namespace OrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class OrderController : Controller
    {
        private readonly IOrdersService _orderService;
        public OrderController(IOrdersService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("CreateOrderWithImages")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> CreateOrderWithImages([FromForm] CreateOrderWithImagesDto dto)
        {
            // Extract UserId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return BadRequest("User ID not found in claims.");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid User ID in claims.");
            }

            // Call the service method
            var result = await _orderService.CreateOrderWithImages(dto, userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok($"Order created and images uploaded to {result.UploadPath}");
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _orderService.DeleteOrder(id);
            if (result)
            {
                return Ok(new { Message = "Order deleted successfully." });
            }
            return NotFound(new { Message = "Order not found." });
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrders()
        {
            var users = await _orderService.GetOrders();

            return Ok(users.ToList());
        }


        [HttpGet("GetOrders_ByStatus")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrdersByStatus(bool finished)
        {
            var orders = await _orderService.GetOrdersByStatus(finished);
            return Ok(orders);
        }

        [HttpGet("GetOrders_ByUsername")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrdersByUsername(string username)
        {
            var orders = await _orderService.GetOrdersByUsername(username);
            return Ok(orders);
        }

        [HttpPut("UpdateOrderStatus/{orderId}")]

        [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] bool status)
        {
            var result = await _orderService.UpdateOrderStatus(orderId, status);
            if (!result)
            {
                return NotFound("Order not found.");
            }

            return Ok("Order status updated successfully.");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("GetOrderStatus/{orderId}")]
        public async Task<IActionResult> GetOrderStatus(int orderId)
        {
            try
            {
                var status = await _orderService.GetOrderStatus(orderId);
                return Ok(new { OrderId = orderId, Status = status });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
