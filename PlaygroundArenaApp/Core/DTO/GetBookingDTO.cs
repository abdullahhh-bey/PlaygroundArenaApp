namespace PlaygroundArenaApp.Core.DTO
{
    public class GetBookingDTO
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
