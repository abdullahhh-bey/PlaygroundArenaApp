namespace PlaygroundArenaApp.Core.Models
{
    public class Court
    {
        public int CourtId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CourtType { get; set; } = string.Empty;
        
        public int ArenaId { get; set; }
        public Arena Arena { get; set; } = null!;

        public ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
