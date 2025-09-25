namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IArenaRepository Arena { get; }
        Task<int> SaveAsync();
    }
}
