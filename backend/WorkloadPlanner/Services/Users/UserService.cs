using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.DTOs.Users;
using WorkloadPlanner.Models;

namespace WorkloadPlanner.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager; 

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserSearchDTO>> SearchUsersAsync(string query, string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return [];
            }

            query = query.ToLower();
            return await _userManager.Users
                .AsNoTracking()
                .Where(u => 
                    u.Id != currentUserId && (
                    u.UserName!.ToLower().Contains(query) ||
                    u.Email!.ToLower().Contains(query)))
                .Take(10)
                .Select(u => new UserSearchDTO
                {
                    Id = u.Id,
                    Username = u.UserName!,
                    Email = u.Email!
                })
                .ToListAsync();
        }
    }
}