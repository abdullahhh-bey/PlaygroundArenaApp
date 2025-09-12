namespace PlaygroundArenaApp.Core.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public int Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        public Booking Booking { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
