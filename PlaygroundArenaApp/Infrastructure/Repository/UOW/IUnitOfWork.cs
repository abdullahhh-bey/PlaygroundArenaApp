using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
using PlaygroundArenaApp.Infrastructure.Repository.BookingRepository;
using PlaygroundArenaApp.Infrastructure.Repository.CourtRepository;
using PlaygroundArenaApp.Infrastructure.Repository.TimeSlotRepository;
namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public ITimeSlotRepository Slot {  get; }
        public IArenaRepository Arena { get; }
        public ICourtRepository Court { get; }
        public IBookingRepository Book { get; }
        Task<int> SaveAsync();
    }
}
