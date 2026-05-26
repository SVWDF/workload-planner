using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WorkloadPlanner.DTOs.Users;
using WorkloadPlanner.Services.Users;

namespace WorkloadPlanner.Controllers
{
    [Route("api/[controller]")]
    [EnableRateLimiting("ApiPolicy")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserSearchDTO>>> SearchUsers([FromQuery] string query)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            return Ok(await _userService.SearchUsersAsync(query, userId));
        }
    }
}
