using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
using PlaygroundArenaApp.Infrastructure.Repository.BookingRepository;
using PlaygroundArenaApp.Infrastructure.Repository.CourtRepository;
namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IArenaRepository Arena { get; }
        public ICourtRepository Court { get; }
        public IBookingRepository Book { get; }
        Task<int> SaveAsync();
    }
}
