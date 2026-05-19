using System.ComponentModel.DataAnnotations.Schema;
using WorkloadPlanner.Enums;

namespace WorkloadPlanner.Models
{
    public class BoardMember
    {
        public int Id { get; set; }
        public int ScrumBoardId { get; set; }
        public ScrumBoard? ScrumBoard { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public BoardRole Role { get; set; }
    }
}