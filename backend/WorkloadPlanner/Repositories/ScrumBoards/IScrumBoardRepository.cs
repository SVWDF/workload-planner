using WorkloadPlanner.Models;

namespace WorkloadPlanner.Repositories.ScrumBoards
{
    public interface IScrumBoardRepository
    {
        Task<IEnumerable<ScrumBoard>> GetScrumBoardsAsync(string userId);
        Task<ScrumBoard?> GetScrumBoardAsync(int id, string userId);
        Task<ScrumBoard> CreateScrumBoardAsync(ScrumBoard board);
        Task AddScrumBoardMembersAsync(IEnumerable<BoardMember> members);
        Task SaveChangesAsync();
    }
}