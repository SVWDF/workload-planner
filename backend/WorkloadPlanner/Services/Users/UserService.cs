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

        public async Task<IEnumerable<UserSearchDTO>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return [];
            }

            return await _userManager.Users
                .Where(u => 
                    u.UserName!.Contains(query) ||
                    u.Email!.Contains(query))
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