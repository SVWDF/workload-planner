using WorkloadPlanner.DTOs.ScrumBoards;

namespace WorkloadPlanner.Services.ScrumBoards
{
    public interface IScrumBoardService
    {
        Task<IEnumerable<ScrumBoardListDTO>> GetScrumBoardsForUserAsync(string userId);
        Task<ScrumBoardDetailDTO> GetScrumBoardByIdAsync(int id, string userId);
        Task<ScrumBoardCreatedDTO> CreateScrumBoardAsync(CreateScrumBoardDTO dto, string userId);
    }
}