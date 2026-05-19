using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Data;
using WorkloadPlanner.Enums;
using WorkloadPlanner.Models;

namespace WorkloadPlanner.Repositories.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly WorkloadPlannerDbContext _context;
        public TicketRepository(WorkloadPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetScrumboardTicketsAsync(int scrumboardId)
        {
            return await _context.Tickets
                .Include(t => t.AssignedUser)
                .Where(t => t.ScrumBoardId == scrumboardId)
                .ToListAsync();
        }

        public async Task<Ticket?> GetTicketAsync(int id)
        {
            return await _context.Tickets
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket> CreateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsScrumboardManagerAsync(int scrumboardId, string userId)
        {
            return await _context.BoardMembers
                .AnyAsync(m => 
                    m.ScrumBoardId == scrumboardId &&
                    m.UserId == userId && 
                    m.Role == BoardRole.Manager);            
        }

        public async Task<bool> IsScrumboardMemberAsync(int scrumboardId, string userId)
        {
            return await _context.BoardMembers
                .AnyAsync(m => 
                    m.ScrumBoardId == scrumboardId &&
                    m.UserId == userId && 
                    m.Role == BoardRole.Member);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}