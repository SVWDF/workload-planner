using WorkloadPlanner.DTOs.ScrumBoards;

namespace WorkloadPlanner.Services.ScrumBoards
{
    public interface IScrumBoardService
    {
        Task<IEnumerable<ScrumBoardListDTO>> GetBoardsForUserAsync(string userId);
        Task<ScrumBoardDetailDTO> GetBoardByIdAsync(int id, string userId);
        Task<ScrumBoardDetailDTO> CreateBoardAsync(CreateScrumBoardDTO dto, string userId);
    }
}