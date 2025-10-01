using PlaygroundArenaApp.Infrastructure.Data;
using PlaygroundArenaApp.Infrastructure.Repository.ArenaRepository;
using PlaygroundArenaApp.Infrastructure.Repository.BookingRepository;
using PlaygroundArenaApp.Infrastructure.Repository.CourtRepository;
using PlaygroundArenaApp.Infrastructure.Repository.TimeSlotRepository;

namespace PlaygroundArenaApp.Infrastructure.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlaygroundArenaDbContext _context;
        public IArenaRepository Arena { get; private set; }
        public ICourtRepository Court { get; private set; }
        public IBookingRepository Book { get; private set; }
        public ITimeSlotRepository Slot { get; private set; }


        public UnitOfWork(PlaygroundArenaDbContext context, ITimeSlotRepository slot , IArenaRepository arena, ICourtRepository court, IBookingRepository book)
        {
            _context = context;
            Arena = arena;
            Court = court;
            Book = book;
            Slot = slot;
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
