using System.ComponentModel.DataAnnotations.Schema;

namespace WorkloadPlanner.Models
{
    public class ScrumBoard
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ManagerId { get; set; } = string.Empty;
        public ApplicationUser? Manager { get; set; }
        public List<BoardMember> Members { get; set; } = [];
        public List<Ticket> Tickets { get; set; } = [];

        [NotMapped]
        public int MemberCount { get; set; }

        [NotMapped]
        public int TicketCount { get; set; }
    }
}