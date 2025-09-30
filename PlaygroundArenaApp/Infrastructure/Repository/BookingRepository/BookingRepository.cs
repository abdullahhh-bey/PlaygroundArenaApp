using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.Models;
using PlaygroundArenaApp.Infrastructure.Data;

namespace PlaygroundArenaApp.Infrastructure.Repository.BookingRepository
{
    public class BookingRepository : IBookingRepository
    {

        private readonly PlaygroundArenaDbContext _context;

        public BookingRepository(PlaygroundArenaDbContext context)
        {
            _context = context;
        }


        public Task AddBooking(Booking booking)
        {
            _context.Bookings.Add(booking);
            return Task.CompletedTask;
        }

        public async Task<List<Booking>> GetAllBookingsWithSlotsWithPayments()
        {
            return await _context.Bookings
                            .Include(s => s.TimeSlots)
                            .Include(p => p.Payment)
                            .ToListAsync();
        }


        public async Task<List<Booking>> GetBookingsByCourtId(int courtId, DateTime date)
        {
             return await _context.Bookings
                            .Where(b => b.CourtId == courtId && b.BookingDate.Date == date.Date)
                            .OrderBy(b => b.StartTime)
                            .ToListAsync();
        }


        public async Task<Booking?> GetBookingWithSlots(int bookingId, int userId)
        {
            return await _context.Bookings
                  .Include(b => b.TimeSlots)
                  .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.UserId == userId);
        }
    }
}
