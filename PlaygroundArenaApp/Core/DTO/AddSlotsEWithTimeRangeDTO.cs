namespace PlaygroundArenaApp.Core.DTO
{
    public class AddSlotsEWithTimeRangeDTO
    {
        public int CourtId { get; set; }
        public DateTime Date { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Price { get; set; }
    }
}
