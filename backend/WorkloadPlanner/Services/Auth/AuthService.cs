using Microsoft.AspNetCore.Identity;
using WorkloadPlanner.DTOs.Auth;
using WorkloadPlanner.Exceptions.Auth;
using WorkloadPlanner.Models;

namespace WorkloadPlanner.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task RegisterAsync(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null) throw new UserAlreadyExistsException(registerDTO.Email);

            var user = new ApplicationUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) throw new PasswordPolicyException(result.Errors.Select(e => e.Description));

            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) throw new InvalidCredentialException();

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded) throw new InvalidCredentialException();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
