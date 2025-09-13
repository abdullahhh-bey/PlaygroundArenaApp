using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddBookingDTO
    {
        public int UserId { get; set; }
        public int CourtId { get; set; }
        public List<int> TimeSlotId { get; set; } = new List<int>();
        public bool BookingStatus { get; set; }
    }
}
