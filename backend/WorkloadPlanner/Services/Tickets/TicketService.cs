using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using WorkloadPlanner.DTOs.Tickets;
using WorkloadPlanner.Enums;
using WorkloadPlanner.Exceptions.Ticket;
using WorkloadPlanner.Hubs;
using WorkloadPlanner.Models;
using WorkloadPlanner.Repositories.Tickets;

namespace WorkloadPlanner.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ScrumboardHub> _hubContext;

        public TicketService(ITicketRepository repository, UserManager<ApplicationUser> userManager, IHubContext<ScrumboardHub> hubContext)
        {
            _repository = repository;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private async Task<Ticket> GetTicketOrThrowAsync(int id)
        {
            return await _repository.GetTicketAsync(id) ?? throw new TicketNotFoundException();
        }

        private async Task EnsureManagerAsync(int scrumboardId, string userId)
        {
            bool isManager = await _repository.IsScrumboardManagerAsync(scrumboardId, userId);
            if (!isManager) throw new UnauthorizedAccessException();
        }

        private async Task EnsureMemberAsync(int scrumboardId, string userId)
        {
            bool isMember = await _repository.IsScrumboardMemberAsync(scrumboardId, userId);
            if (!isMember) throw new UnauthorizedAccessException();
        }

        private async Task EnsureScrumBoardAccessAsync(int scrumboardId, string userId)
        {
            bool isManager = await _repository.IsScrumboardManagerAsync(scrumboardId, userId);
            bool isMember = await _repository.IsScrumboardMemberAsync(scrumboardId, userId);
            if (!isManager && !isMember) throw new UnauthorizedAccessException();
        }

        private async Task NotifyScrumboardAsync(int scrumboardId, string eventName, object payload)
        {
            await _hubContext
                .Clients
                .Group($"scrumboard-{scrumboardId}")
                .SendAsync(eventName, payload);
        }

        private TicketDTO MapTicketDTO(Ticket ticket)
        {
            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                AssignedUser = ticket.AssignedUser?.UserName  
            };
        }

        public async Task<IEnumerable<TicketDTO>> GetScrumboardTicketsAsync(int scrumboardId, string userId)
        {
            await EnsureScrumBoardAccessAsync(scrumboardId, userId);
            IEnumerable<Ticket> tickets = await _repository.GetScrumboardTicketsAsync(scrumboardId);
            return tickets
                .Select(t => MapTicketDTO(t));
        }

        public async Task<TicketDTO> CreateTicketAsync(CreateTicketDTO dto, string userId)
        {
            await EnsureManagerAsync(dto.ScrumBoardId, userId);

            Ticket ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                ScrumBoardId = dto.ScrumBoardId,
                Priority = dto.Priority,
                Status = TicketStatus.Todo,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };
            await _repository.CreateTicketAsync(ticket);

            TicketDTO ticketDTO = MapTicketDTO(ticket);

            await NotifyScrumboardAsync(ticket.ScrumBoardId, "TicketCreated", ticketDTO);
            
            return ticketDTO;
        }

        public async Task<TicketDTO> UpdateTicketAsync(int id, UpdateTicketDTO dto, string userId)
        {
            Ticket ticket = await GetTicketOrThrowAsync(id);

            await EnsureManagerAsync(ticket.ScrumBoardId, userId);

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.Priority = dto.Priority;
            await _repository.UpdateTicketAsync(ticket);

            TicketDTO ticketDTO = MapTicketDTO(ticket);

            await NotifyScrumboardAsync(ticket.ScrumBoardId, "TicketUpdated", ticketDTO);
            
            return ticketDTO;
        }

        public async Task DeleteTicketAsync(int id, string userId)
        {
            Ticket ticket = await GetTicketOrThrowAsync(id);

            await EnsureManagerAsync(ticket.ScrumBoardId, userId);

            int scrumboardId = ticket.ScrumBoardId;
            int ticketId = ticket.Id;

            await _repository.DeleteTicketAsync(ticket);

            await NotifyScrumboardAsync(scrumboardId, "TicketDeleted", ticketId);
        }

        public async Task<TicketDTO> AssignSelfToTicketAsync(int id, string userId)
        {
            Ticket ticket = await GetTicketOrThrowAsync(id);

            await EnsureMemberAsync(ticket.ScrumBoardId, userId);

            ticket.AssignedUserId = userId;
            await _repository.SaveChangesAsync();

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            TicketDTO ticketDTO = MapTicketDTO(ticket);
            ticketDTO.AssignedUser = user?.UserName;

            await NotifyScrumboardAsync(ticket.ScrumBoardId, "TicketAssigned", ticketDTO);
            
            return ticketDTO;
        }

        public async Task<TicketDTO> UpdateStatusAsync(int id, UpdateTicketStatusDTO dto, string userId)
        {
            Ticket ticket = await GetTicketOrThrowAsync(id);

            await EnsureMemberAsync(ticket.ScrumBoardId, userId);

            ticket.Status = dto.Status;
            await _repository.SaveChangesAsync();

            TicketDTO ticketDTO = MapTicketDTO(ticket);

            await NotifyScrumboardAsync(ticket.ScrumBoardId, "TicketStatusChanged", ticketDTO);
            
            return ticketDTO;
        }
    }
}