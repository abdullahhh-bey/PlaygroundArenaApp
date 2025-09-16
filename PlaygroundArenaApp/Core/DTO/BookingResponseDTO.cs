namespace PlaygroundArenaApp.Core.DTO
{
    public class BookingResponseDTO
    {
        public string Message { get; set } = string.Empty;
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int CourtId { get; set; }
    }
}
