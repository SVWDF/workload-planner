using Microsoft.AspNetCore.Identity;
using WorkloadPlanner.DTOs;
using WorkloadPlanner.Models;
using WorkloadPlanner.Services.Jwt;

namespace WorkloadPlanner.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);

            if (existingUser != null)
            {
                return null;
            }

            var user = new ApplicationUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return null;

            return GenerateToken(user);
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return null;
            }

            return GenerateToken(user);
        }
        
        private AuthResponseDTO GenerateToken(ApplicationUser user)
        {
            var token = _tokenService.GenerateToken(user);
            var expiryMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_HOURS") ?? "3");

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ExpiresAt = DateTime.UtcNow.AddHours(expiryMinutes)
            };
        }
    }
}
