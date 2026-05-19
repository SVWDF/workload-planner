using WorkloadPlanner.Repositories.ScrumBoards;
using WorkloadPlanner.DTOs.ScrumBoards;
using WorkloadPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Constants;
using WorkloadPlanner.Exceptions.Scrumboard;
using WorkloadPlanner.Enums;

namespace WorkloadPlanner.Services.ScrumBoards
{
    public class ScrumBoardService : IScrumBoardService
    {
        private readonly IScrumBoardRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScrumBoardService(IScrumBoardRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ScrumBoardListDTO>> GetBoardsForUserAsync(string userId)
        {
            var boards = await _repository.GetBoardsAsync(userId);
            return boards.Select(board => new ScrumBoardListDTO
            {
                Id = board.Id,
                Name = board.Name,
                Color = board.Color,
                Members = board.MemberCount,
                Tickets = board.TicketCount
            });
        }

        public async Task<ScrumBoardDetailDTO> GetBoardByIdAsync(int id, string userId)
        {
            var board = await _repository.GetBoardAsync(id, userId);
            if (board == null) throw new ScrumboardNotFoundException();
            
            return new ScrumBoardDetailDTO
            {
                Id = board.Id,
                Name = board.Name,
                Members = board.MemberCount,
                Tickets = board.TicketCount,
                IsManager = board.ManagerId == userId
            };
        }

        public async Task<ScrumBoardDetailDTO> CreateBoardAsync(CreateScrumBoardDTO dto, string userId)
        {
            string color = dto.Color;
            if (string.IsNullOrWhiteSpace(color) || !BoardColors.Colors.Contains(color))
            {
                color = BoardColors.GetRandomColor();
            }

            var board = new ScrumBoard
            {
                Name = dto.Name,
                Color = color,
                ManagerId = userId
            };

            board = await _repository.CreateBoardAsync(board);

            var members = new List<BoardMember>
            {
                new()
                {
                    ScrumBoardId = board.Id,
                    UserId = userId,
                    Role = BoardRole.Manager      
                }
            };

            var validUserIds = await _userManager.Users
                .Where(u => dto.MemberIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            members.AddRange(
                validUserIds
                    .Distinct()
                    .Where(id => id != userId)
                    .Select(id => new BoardMember
                    {
                        ScrumBoardId = board.Id,
                        UserId = id,
                        Role = BoardRole.Member
                    })
            );

            await _repository.AddBoardMembersAsync(members);

            return new ScrumBoardDetailDTO { Id = board.Id, Name = board.Name };
        }
    }
}