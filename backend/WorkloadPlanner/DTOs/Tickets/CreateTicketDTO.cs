using System.ComponentModel.DataAnnotations;
using WorkloadPlanner.Enums;

namespace WorkloadPlanner.DTOs.Tickets
{
    public class CreateTicketDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description {get; set; } = string.Empty;
        public int ScrumBoardId { get; set; }
        public TicketPriority Priority { get; set; }
    }
}