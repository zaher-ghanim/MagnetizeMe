namespace Users.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Users.Interfaces;
    using UsersAPI.DTOs;

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
            var authToken = _useresService.VerifyUser(request.Username, request.Password);
            if (string.IsNullOrEmpty(authToken))
            {
                return Unauthorized();
            }
            return Ok(authToken);
        }


        // Public endpoint: Anyone can add a user
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method to add the user
            var result = await _useresService.AddUser(userRequest);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            // Return the UserResponse
            return Ok(result.UserResponse);
        }

        // Only admins can get all users
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUsers()
        {
            var userResponses = await _useresService.GetUsers();
            return Ok(userResponses);
        }

        // Only admins can get a user by ID
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userResponse = await _useresService.GetUserById(id);

            if (userResponse == null)
            {
                return NotFound("User not found.");
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method to update the user
            var result = await _useresService.UpdateUser(id, userRequest, User);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.UserResponse);
        }

    }
}
