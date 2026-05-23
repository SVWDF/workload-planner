using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Data;
using WorkloadPlanner.Models;

namespace WorkloadPlanner.Repositories.ScrumBoards
{
    public class ScrumBoardRepository : IScrumBoardRepository
    {
        private readonly WorkloadPlannerDbContext _context;

        public ScrumBoardRepository(WorkloadPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ScrumBoard>> GetScrumBoardsAsync(string userId)
        {
            return await _context.ScrumBoards
                .AsNoTracking()
                .Where(b => b.Members.Any(bm => bm.UserId == userId))
                .Select(b => new ScrumBoard
                {
                    Id = b.Id,
                    Name = b.Name,
                    Color = b.Color,
                    MemberCount = b.Members.Count(),
                    TicketCount = b.Tickets.Count()
                })
                .ToListAsync();
        }

        public async Task<ScrumBoard?> GetScrumBoardAsync(int id, string userId)
        {
            return await _context.ScrumBoards
                .AsNoTracking()
                .Where(b => b.Id == id && b.Members.Any(m => m.UserId == userId))
                .Select(b => new ScrumBoard
                {
                    Id = b.Id,
                    Name = b.Name,
                    MemberCount = b.Members.Count(),
                    TicketCount = b.Tickets.Count(),
                    ManagerId = b.ManagerId
                })
                .FirstOrDefaultAsync();
        }

        public Task<ScrumBoard> CreateScrumBoardAsync(ScrumBoard board)
        {
            _context.ScrumBoards.Add(board);
            return Task.FromResult(board);
        }

        public async Task AddScrumBoardMembersAsync(IEnumerable<BoardMember> members)
        {
            await _context.BoardMembers.AddRangeAsync(members);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}