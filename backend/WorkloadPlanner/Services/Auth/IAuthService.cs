using WorkloadPlanner.DTOs;

namespace WorkloadPlanner.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDTO?> RegisterAsync(RegisterDTO registerDTO);
    }
}
