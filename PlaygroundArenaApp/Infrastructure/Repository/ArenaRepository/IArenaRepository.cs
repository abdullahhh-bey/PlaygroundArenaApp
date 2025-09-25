using Microsoft.AspNetCore.Identity;
using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository
{
    public interface IArenaRepository
    {
        Task<List<Arena>> GetArenaList();
        Task<Arena> GetArenaByIdWithCourt(int id);
        Task AddArena(Arena arena);
        Task<Arena> GetArenaById(int id);
        Task<Arena> GetArenaByIdWithCourtAndSlots(int id);
        Task DeleteArena(Arena arena);
    }
}
