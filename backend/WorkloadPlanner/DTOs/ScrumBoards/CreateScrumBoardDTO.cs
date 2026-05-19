using System.ComponentModel.DataAnnotations;

namespace WorkloadPlanner.DTOs.ScrumBoards
{
    public class CreateScrumBoardDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        public List<string> MemberIds { get; set; } = [];
    }
}