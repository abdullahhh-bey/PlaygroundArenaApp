namespace PlaygroundArenaApp.Core.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int Amount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
