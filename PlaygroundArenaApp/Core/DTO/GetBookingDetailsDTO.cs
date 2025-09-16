namespace PlaygroundArenaApp.Core.DTO
{
    public class GetBookingDetailsDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int CourtId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string BookingStatus { get; set; } = "Pending";
        public PaymentDTO Payments { get; set; }

    }
}
