using Microsoft.AspNetCore.SignalR;
using WorkloadPlanner.DTOs.Tickets;
using WorkloadPlanner.Enums;
using WorkloadPlanner.Hubs;
using WorkloadPlanner.Models;
using WorkloadPlanner.Repositories.Tickets;

namespace WorkloadPlanner.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;
        private readonly IHubContext<ScrumboardHub> _hubContext;

        public TicketService(ITicketRepository repository, IHubContext<ScrumboardHub> hubContext)
        {
            _repository = repository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<TicketDTO>> GetScrumboardTicketsAsync(int scrumboardId, string userId)
        {
            var tickets = await _repository.GetScrumboardTicketsAsync(scrumboardId);
            return tickets
                .Select(t =>
                    new TicketDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Status = t.Status,
                        Priority = t.Priority,
                        AssignedUser = t.AssignedUser?.UserName
                    });
        }

        public async Task<TicketDTO> CreateTicketAsync(CreateTicketDTO dto, string userId)
        {
            var isManager = await _repository.IsScrumboardManagerAsync(dto.ScrumBoardId, userId);
            if (!isManager) throw new UnauthorizedAccessException();

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                ScrumBoardId = dto.ScrumBoardId,
                Priority = dto.Priority,
                Status = TicketStatus.Todo,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId
            };

            ticket = await _repository.CreateTicketAsync(ticket);
            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status
            };
        }

        public async Task<TicketDTO> UpdateTicketAsync(int id, UpdateTicketDTO dto, string userId)
        {
            var ticket = await _repository.GetTicketAsync(id);
            if (ticket == null) throw new KeyNotFoundException();

            var isManager = await _repository.IsScrumboardManagerAsync(ticket.ScrumBoardId, userId);
            if (!isManager) throw new UnauthorizedAccessException();

            ticket.Title = dto.Title;
            ticket.Description = dto.Description;
            ticket.Priority = dto.Priority;
            await _repository.UpdateTicketAsync(ticket);

            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status
            };
        }

        public async Task DeleteTicketAsync(int id, string userId)
        {
            var ticket = await _repository.GetTicketAsync(id);
            if (ticket == null) throw new KeyNotFoundException();

            var isManager = await _repository.IsScrumboardManagerAsync(ticket.ScrumBoardId, userId);
            if (!isManager) throw new UnauthorizedAccessException();

            await _repository.DeleteTicketAsync(ticket);
        }

        public async Task<TicketDTO> AssignSelfToTicketAsync(int id, string userId)
        {
            var ticket = await _repository.GetTicketAsync(id);
            if (ticket == null) throw new KeyNotFoundException();

            var isMember = await _repository.IsScrumboardMemberAsync(ticket.ScrumBoardId, userId);
            if (!isMember) throw new UnauthorizedAccessException();

            ticket.AssignedUserId = userId;
            await _repository.SaveChangesAsync();

            ticket = await _repository.GetTicketAsync(id);
            var dto = new TicketDTO
            {
                Id = ticket!.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Status = ticket.Status,
                AssignedUser = ticket.AssignedUser?.UserName
            };

            await _hubContext
                .Clients
                .Group($"scrumboard-{ticket.ScrumBoardId}")
                .SendAsync("TicketAssigned", dto);

            return dto;
        }

        public async Task<TicketDTO> UpdateStatusAsync(int id, UpdateTicketStatusDTO dto, string userId)
        {
            var ticket = await _repository.GetTicketAsync(id);
            if (ticket == null) throw new KeyNotFoundException();

            var isMember = await _repository.IsScrumboardMemberAsync(ticket.ScrumBoardId, userId);
            if (!isMember) throw new UnauthorizedAccessException();

            ticket.Status = dto.Status;
            await _repository.SaveChangesAsync();

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
    }
}