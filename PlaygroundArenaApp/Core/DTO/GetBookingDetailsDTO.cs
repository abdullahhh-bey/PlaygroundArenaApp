namespace PlaygroundArenaApp.Core.DTO
{
    public class GetBookingDetailsDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int CourtId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public string BookingStatus { get; set; } = "Pending";

        public List<TimeSlotsDTO> TimeSlots { get; set; } = new List<TimeSlotsDTO>();
        public PaymentDTO Payments { get; set; }

    }
}
