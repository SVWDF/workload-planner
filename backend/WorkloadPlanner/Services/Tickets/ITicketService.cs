using WorkloadPlanner.DTOs.Tickets;

namespace WorkloadPlanner.Services.Tickets
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDTO>> GetScrumboardTicketsAsync(int scrumboardId, string userId);
        Task<TicketDTO> CreateTicketAsync(CreateTicketDTO dto, string userId);
        Task<TicketDTO> UpdateTicketAsync(int id, UpdateTicketDTO dto, string userId);
        Task DeleteTicketAsync(int id, string userId);
        Task<TicketDTO> AssignSelfToTicketAsync(int id, string userId);
        Task<TicketDTO> UpdateStatusAsync(int id, UpdateTicketStatusDTO dto, string userId);
    }
}