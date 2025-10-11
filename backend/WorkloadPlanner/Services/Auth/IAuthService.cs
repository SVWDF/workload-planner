using WorkloadPlanner.DTOs;

namespace WorkloadPlanner.Services.Auth
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO registerDTO);
        Task LoginAsync(LoginDTO loginDTO);
        Task LogoutAsync();
    }
}
