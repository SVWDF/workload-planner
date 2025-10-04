using WorkloadPlanner.DTOs;

namespace WorkloadPlanner.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
    }
}
