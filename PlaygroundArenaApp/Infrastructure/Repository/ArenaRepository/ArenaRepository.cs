using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository
{
    public class ArenaRepository : IArenaRepository
    {
        private readonly PlaygroundArenaDbContext _context;
        public ArenaRepository(PlaygroundArenaDbContext context)
        {
            _context = context;
        }


        public async Task<List<Arena>> GetArenaList()
        {
            return await _context.Arenas.ToListAsync();
        }


        public async Task<Arena> GetArenaByIdWithCourt(int id)
        {
            return await _context.Arenas
                         .Include(a => a.Courts)
                         .FirstOrDefaultAsync(a => a.ArenaId == id);
        }


        public async Task<IdentityResult> AddArena( Arena arena)
        {
            return await _context.Arenas.AddAsync(arena);
        }


        public async Task<Arena> GetArenaById(int id)
        {
            return await _context.Arenas.FirstOrDefault(a => a.ArenaId == id);
        }


        public async Task<Arena> GetArenaByIdWithCourtAndSlots(int id)
        {
            return await _context.Arenas
                        .Include(c => c.Courts)
                            .ThenInclude(t => t.TimeSlots)
                        .FirstOrDefaultAsync(a => a.ArenaId == id);
        }



        public async Task<IdentityResult> DeleteArena(Arena arena)
        {
            return await _context.Arenas.Remove(arena);
        }



    }
}
