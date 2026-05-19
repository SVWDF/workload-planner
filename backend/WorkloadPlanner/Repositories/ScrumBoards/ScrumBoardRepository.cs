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

        public async Task<IEnumerable<ScrumBoard>> GetBoardsAsync(string userId)
        {
            return await _context.ScrumBoards
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

        public async Task<ScrumBoard?> GetBoardAsync(int id, string userId)
        {
            return await _context.BoardMembers
                .Where(m => m.UserId == userId && m.ScrumBoardId == id)
                .Select(m => new ScrumBoard
                {
                    Id = m.ScrumBoard!.Id,
                    Name = m.ScrumBoard.Name,
                    MemberCount = m.ScrumBoard.Members.Count(),
                    TicketCount = m.ScrumBoard.Tickets.Count(),
                    ManagerId = m.ScrumBoard.ManagerId
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ScrumBoard> CreateBoardAsync(ScrumBoard board)
        {
            _context.ScrumBoards.Add(board);
            await _context.SaveChangesAsync();
            return board;
        }

        public async Task AddBoardMembersAsync(IEnumerable<BoardMember> members)
        {
            await _context.BoardMembers.AddRangeAsync(members);
            await _context.SaveChangesAsync();
        }
    }
}