using WorkloadPlanner.Enums;

namespace WorkloadPlanner.DTOs.Tickets
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
        public string? AssignedUser { get; set; }
        public TicketPriority Priority { get; set; }
    }
}