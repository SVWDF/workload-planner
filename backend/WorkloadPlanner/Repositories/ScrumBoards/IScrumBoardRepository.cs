using WorkloadPlanner.Models;

namespace WorkloadPlanner.Repositories.ScrumBoards
{
    public interface IScrumBoardRepository
    {
        Task<IEnumerable<ScrumBoard>> GetBoardsAsync(string userId);
        Task<ScrumBoard?> GetBoardAsync(int id, string userId);
        Task<ScrumBoard> CreateBoardAsync(ScrumBoard board);
        Task AddBoardMembersAsync(IEnumerable<BoardMember> members);
    }
}