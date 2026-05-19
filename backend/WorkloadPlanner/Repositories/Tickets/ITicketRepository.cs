using WorkloadPlanner.Models;

namespace WorkloadPlanner.Repositories.Tickets
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetScrumboardTicketsAsync(int scrumboardId);
        Task<Ticket?> GetTicketAsync(int id);
        Task<Ticket> CreateTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(Ticket ticket);
        Task<bool> IsScrumboardManagerAsync(int scrumboardId, string userId);
        Task<bool> IsScrumboardMemberAsync(int scrumboardId, string userId);
        Task SaveChangesAsync();
    }
}