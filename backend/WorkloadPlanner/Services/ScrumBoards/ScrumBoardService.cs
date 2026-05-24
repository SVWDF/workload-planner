using WorkloadPlanner.Repositories.ScrumBoards;
using WorkloadPlanner.DTOs.ScrumBoards;
using WorkloadPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Constants;
using WorkloadPlanner.Exceptions.Scrumboard;
using WorkloadPlanner.Enums;
using Microsoft.AspNetCore.SignalR;
using WorkloadPlanner.Hubs;

namespace WorkloadPlanner.Services.ScrumBoards
{
    public class ScrumBoardService : IScrumBoardService
    {
        private readonly IScrumBoardRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ScrumboardHub> _hubContext;

        public ScrumBoardService(IScrumBoardRepository repository, UserManager<ApplicationUser> userManager, IHubContext<ScrumboardHub> hubContext)
        {
            _repository = repository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private string ValidateBoardColor(string color)
        {
            //Check color is not empty and exists inside list, else assign one on random
            if (string.IsNullOrWhiteSpace(color) || !BoardColors.Colors.Contains(color))
            {
                color = BoardColors.GetRandomColor();
            }
            return color;
        }

        private async Task<List<BoardMember>> BuildBoardMembersAsync(CreateScrumBoardDTO dto, ScrumBoard board, string userId)
        {
            //Setup list members with default user that created the scrumboard added as manager
            List<BoardMember> members = new List<BoardMember>
            {
                new()
                {
                    ScrumBoard = board,
                    UserId = userId,
                    Role = BoardRole.Manager      
                }
            };

            //Add any members to scrumboard that were selected at creation
            dto.MemberIds = dto.MemberIds.Distinct().ToList();
            List<string> validUserIds = await _userManager.Users
                .Where(u => dto.MemberIds.Contains(u.Id))
                .Select(u => u.Id)
                .ToListAsync();

            members.AddRange(
                validUserIds
                    .Where(id => id != userId)
                    .Select(id => new BoardMember
                    {
                        ScrumBoard = board,
                        UserId = id,
                        Role = BoardRole.Member
                    })
            );

            return members;
        }

        public async Task<IEnumerable<ScrumBoardListDTO>> GetScrumBoardsForUserAsync(string userId)
        {
            IEnumerable<ScrumBoard> boards = await _repository.GetScrumBoardsAsync(userId);
            return boards.Select(board => new ScrumBoardListDTO
            {
                Id = board.Id,
                Name = board.Name,
                Color = board.Color,
                Members = board.MemberCount,
                Tickets = board.TicketCount
            });
        }

        public async Task<ScrumBoardDetailDTO> GetScrumBoardByIdAsync(int id, string userId)
        {
            ScrumBoard? board = await _repository.GetScrumBoardAsync(id, userId);
            if (board == null) throw new ScrumBoardNotFoundException();
            
            return new ScrumBoardDetailDTO
            {
                Id = board.Id,
                Name = board.Name,
                Members = board.MemberCount,
                Tickets = board.TicketCount,
                IsManager = board.ManagerId == userId
            };
        }

        

        public async Task<ScrumBoardCreatedDTO> CreateScrumBoardAsync(CreateScrumBoardDTO dto, string userId)
        {
            //Validate color code for board
            string color = ValidateBoardColor(dto.Color);

            //Create new scrumboard
            ScrumBoard board = new ScrumBoard
            {
                Name = dto.Name,
                Color = color,
                ManagerId = userId
            };
            await _repository.CreateScrumBoardAsync(board);

            //Build members list
            List<BoardMember> members = await BuildBoardMembersAsync(dto, board, userId);            
            await _repository.AddScrumBoardMembersAsync(members);

            //Save all at once
            await _repository.SaveChangesAsync();

            //Update users scrumboard list that were added
            ScrumBoardListDTO scrumboardDTO = new ScrumBoardListDTO
            {
                Id = board.Id,
                Name = board.Name,
                Color = board.Color,
                Members = members.Count,
                Tickets = 0  
            };

            await Task.WhenAll(members
                .Select(m => _hubContext
                    .Clients
                    .User(m.UserId)
                    .SendAsync("ScrumboardCreated", scrumboardDTO)));

            //Return newly created scrumboard
            return new ScrumBoardCreatedDTO { Id = board.Id, Name = board.Name };
        }
    }
}