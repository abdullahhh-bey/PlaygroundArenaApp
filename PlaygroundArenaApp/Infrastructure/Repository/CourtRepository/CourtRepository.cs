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

        public Task DeleteCourtWithSlots(Court court)
        {
             _context.Courts.RemoveRange(court);
            return Task.CompletedTask;
        }

        public async Task<Court> GetCourtById(int id)
        {
            return await _context.Courts.FindAsync(id);
        }

        public Task<Court> GetCourtByIdWithSlots(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Court>> GetCourtByTypeWithArenaId(int id, string type)
        {
            throw new NotImplementedException();
        }

        public Task<List<Court>> GetCourtList()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCourtExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
