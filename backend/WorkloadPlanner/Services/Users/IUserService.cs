using WorkloadPlanner.DTOs.Users;

namespace WorkloadPlanner.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserSearchDTO>> SearchUsersAsync(string query, string currentUserId);
    }
}