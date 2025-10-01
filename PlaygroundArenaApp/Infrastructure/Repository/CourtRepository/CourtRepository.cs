using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Infrastructure.Repository.CourtRepository
{
    public class CourtRepository : ICourtRepository
    {
        private readonly PlaygroundArenaDbContext _context;
        public CourtRepository(PlaygroundArenaDbContext context)
        {
            _context = context;
        }


        public async Task AddCourtAsync(Court court)
        {
            await _context.Courts.AddAsync(court);
        }

        public async Task<bool> CheckCourtByNameAndArenaId(string name, int arenaId)
        {
            var check = await _context.Courts.AnyAsync(c => c.Name == name && c.ArenaId == arenaId);
            if(check)
                return true;
            else
                return false;
        }

        public Task DeleteCourtWithSlots(ICollection<Court> court)
        {
             _context.Courts.RemoveRange(court);
            return Task.CompletedTask;
        }

        public async Task<Court?> GetCourtById(int id)
        {
            return await _context.Courts.FindAsync(id);
        }

        public async Task<Court?> GetCourtByIdWithSlots(int id)
        {
            return await _context.Courts
                         .Include(t => t.TimeSlots)
                         .FirstOrDefaultAsync(c => c.CourtId == id);
        }

        public async Task<List<Court>> GetCourtByTypeWithArenaId(int id, string type)
        {
            return await _context.Courts
                            .Where(c => c.ArenaId == id && c.CourtType.ToLower() == type.ToLower())
                            .OrderBy(c => c.CourtId)
                            .ToListAsync();
        }

        public async Task<List<Court>> GetCourtList()
        {
            return await _context.Courts.ToListAsync();
        }

        public async Task<bool> IsCourtExists(int id)
        {
            return await _context.Courts.AnyAsync(c => c.CourtId == id);
        }



    }
}
