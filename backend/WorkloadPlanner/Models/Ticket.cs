using System.ComponentModel.DataAnnotations.Schema;
using WorkloadPlanner.Enums;

namespace WorkloadPlanner.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(20)")]
        public TicketStatus Status { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public TicketPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ScrumBoardId { get; set; }
        public ScrumBoard ScrumBoard { get; set; } = null!;
        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public ApplicationUser CreatedBy { get; set; } = null!;
    }
}