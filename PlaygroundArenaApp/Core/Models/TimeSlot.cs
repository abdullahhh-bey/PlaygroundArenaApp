namespace PlaygroundArenaApp.Core.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date {  get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
        public int CourtId { get; set; }
        public Court Court { get; set; } = null!;

        public int? BookingId { get; set; }
        public Booking? Booking { get; set; }

    }
}
