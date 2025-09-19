namespace PlaygroundArenaApp.Core.DTO
{
    public class CourtSlotsDTO
    {
        public string Name { get; set; }
        public List<SlotsDTO> Slots { get; set; } = new List<SlotsDTO>();
    }
}
