using WorkloadPlanner.Models;

namespace WorkloadPlanner.Services.Jwt
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
