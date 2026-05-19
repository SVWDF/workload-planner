using System.ComponentModel.DataAnnotations;
using WorkloadPlanner.Enums;

namespace WorkloadPlanner.DTOs.Tickets
{
    public class UpdateTicketDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketPriority Priority { get; set; }
    }
}