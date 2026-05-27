namespace WorkloadPlanner.DTOs.ScrumBoards
{
    public class ScrumBoardDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Members { get; set; }
        public int Tickets { get; set; }
        public bool IsManager { get; set; }
    }
}