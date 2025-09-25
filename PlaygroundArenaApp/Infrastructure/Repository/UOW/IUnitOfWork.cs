using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IArenaRepository Arena { get; }
        Task<int> SaveAsync();
    }
}
