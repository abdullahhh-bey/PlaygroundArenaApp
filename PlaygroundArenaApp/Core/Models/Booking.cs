namespace PlaygroundArenaApp.Core.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int CourtId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string BookingStatus { get; set; } = "Pending";


        public Payment? Payment { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public Court Court { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
