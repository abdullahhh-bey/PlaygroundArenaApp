using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Repository.CourtRepository
{
    public interface ICourtRepository
    {
        Task<List<Court>> GetCourtList();
        Task<Court> GetCourtById(int id);
        Task<Court> GetCourtByIdWithSlots(int id); //First  or default
        Task<List<Court>> GetCourtByTypeWithArenaId(int id, string type);
        Task<bool> IsCourtExists(int id); //Any
        Task<bool> CheckCourtByNameAndArenaId(string name, int arenaId);
        Task AddCourtAsync(Court court);
        Task DeleteCourtWithSlots(Court court); //Remove range
    }
}
