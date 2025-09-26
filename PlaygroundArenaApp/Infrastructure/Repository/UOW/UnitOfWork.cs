using PlaygroundArenaApp.Infrastructure.Data;
using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
using PlaygroundArenaApp.Infrastructure.Repository.CourtRepository;

namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaygroundArenaDbContext _context;
        public IArenaRepository Arena { get; private set; }
        public ICourtRepository Court { get; private set; }

        public UnitOfWork(PlaygroundArenaDbContext context, IArenaRepository arena, ICourtRepository court)
        {
            _context = context;
            Arena = arena;
            Court = court;
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() {
             _context.Dispose();
        }


    }
}
