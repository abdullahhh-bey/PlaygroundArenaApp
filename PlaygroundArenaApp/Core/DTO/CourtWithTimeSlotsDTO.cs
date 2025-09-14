namespace PlaygroundArenaApp.Core.DTO
{
    public class CourtWithTimeSlotsDTO
    {
        public int CourtId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public List<TimeSlotsDTO> TimeSlots { get; set; } = new List<TimeSlotsDTO>();

    }
}
