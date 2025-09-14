namespace PlaygroundArenaApp.Core.DTO
{
    public class TimeSlotsDTO
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public int Price { get; set; }
    }
}
