using Microsoft.AspNetCore.Identity;

namespace PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository
{
    public interface IArenaRepository
    {
        Task<List<Arena>> GetArenaList();
        Task<Arena> GetArenaByIdWithCourt(int id):
        Task<IdentityResult> AddArena(Arena arena):
        Task<Arena> GetArenaById(int id):
        Task<Arena> GetArenaByIdWithCourtAndSlots(int id):
        Task<IdentityResult> DeleteArena(Arena arena);
    }
}
