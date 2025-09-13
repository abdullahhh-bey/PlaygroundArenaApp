namespace PlaygroundArenaApp.Core.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public int CourtId { get; set; }
        public Court Court { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }

    }
}
