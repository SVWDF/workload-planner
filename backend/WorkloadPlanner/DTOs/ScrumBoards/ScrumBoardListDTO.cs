namespace WorkloadPlanner.DTOs.ScrumBoards
{
    public class ScrumBoardListDTO
    {
        public int Id { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Members { get; set; }
        public int Tickets { get; set; }
    }
}