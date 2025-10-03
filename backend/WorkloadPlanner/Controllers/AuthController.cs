using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkloadPlanner.DTOs;
using WorkloadPlanner.Models;
using WorkloadPlanner.Services.Auth;
using WorkloadPlanner.Services.Jwt;

namespace WorkloadPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerUserDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerUserDTO);

            if (result == null) return BadRequest(new { message = "User with this email already exists" });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDTO);

            if (result == null) return Unauthorized(new { message = "Invalid email or password" });

            return Ok(result);
        }
    }
}
