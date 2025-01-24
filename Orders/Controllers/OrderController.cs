using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryAPI.Models;
using RepositoryAPI.Models.OrdersDTO;
using Users.Interfaces;

namespace OrdersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]

    public class OrderController : Controller
    {
        private readonly IOrdersService _orderService;
        public OrderController(IOrdersService orderService)
        {
            _orderService = orderService;
        }

        //[HttpPost("TestFileUpload")]
        //[Consumes("multipart/form-data")]
        //public IActionResult TestFileUpload([FromForm] List<IFormFile> files)
        //{
        //    return Ok(new { Count = files.Count });
        //}



        [HttpPost("CreateOrderWithImages")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateOrderWithImages([FromForm] CreateOrderWithImagesDto dto)
        {
            if (dto.Images == null || dto.Images.Count == 0)
            {
                return BadRequest("No images were uploaded.");
            }

            string username = await _orderService.GetUsernameFromUserId(dto.Order.UserId);
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Invalid user ID.");
            }

            Size size = _orderService.GetOrdersSize(dto.Order.SizeId).Result;
            int Images_Qty = dto.Images.Count;
            int step_qty = size.StepQuantity;
            int min_qty = size.Quantity;
            decimal actual_price = size.Price;
            decimal price = Math.Ceiling((decimal)Images_Qty / min_qty) * actual_price;

            Order newOrder = new Order()
            {
                UserId = dto.Order.UserId,
                SizeId = dto.Order.SizeId,
                Address = dto.Order.Address,
                Phone = dto.Order.Phone,
                Qty = dto.Images.Count,
                Price = price

            };

            var result = await _orderService.CreateOrder(newOrder);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            // Create the upload path using the username and order ID
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", username, result.orderid.ToString());


            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            int i = 1;
            // Loop through each uploaded file and save it to the constructed path
            foreach (var file in dto.Images)
            {
                if (file.Length > 0)
                {
                    //string dimension = size.Dimension;

                    var filePath = Path.Combine(uploadPath, i.ToString() + "-" + file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    i++;
                }
            }

            // Return success message
            return Ok($"Order created and images uploaded to {uploadPath}");
        }

        [HttpDelete("{id}")]
        //[Authorize(Policy = "AdminOnly")] // Only admins can delete users
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
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrders()
        {
            var users = await _orderService.GetOrders();

            return Ok(users.ToList());
        }


        [HttpGet("GetOrders_ByStatus")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetOrdersByStatus(bool finished)
        {
            var orders = await _orderService.GetOrdersByStatus(finished);
            return Ok(orders);
        }

        [HttpGet("GetOrders_ByUsername")]
        public async Task<IActionResult> GetOrdersByUsername(string username)
        {
            var orders = await _orderService.GetOrdersByUsername(username);
            return Ok(orders);
        }

    }
}
