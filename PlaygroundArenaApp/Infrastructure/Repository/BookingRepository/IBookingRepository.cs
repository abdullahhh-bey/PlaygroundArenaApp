using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Repository.BookingRepository
{
    public interface IBookingRepository
    {
        Task AddBooking(Booking booking);
        Task<Booking?> GetBookingWithSlots(int bookingId, int userId);
        Task<List<Booking>> GetAllBookingsWithSlotsWithPayments();
        Task<List<Booking>> GetBookingsByCourtId(int courtId, DateTime date);
    }
}
