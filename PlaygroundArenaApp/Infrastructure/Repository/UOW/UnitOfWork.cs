using PlaygroundArenaApp.Infrastructure.Data;
using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;

namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaygroundArenaDbContext _context;
        private readonly IArenaRepository Arena { get; };

        public UnitOfWork(PlaygroundArenaDbContext context, IArenaRepository arena)
        {
            _context = context;
            Arena = arena;
        }


        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose() {
            return _context.Dispose();
        }


    }
}
