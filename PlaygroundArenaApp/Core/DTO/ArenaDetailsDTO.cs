namespace PlaygroundArenaApp.Core.DTO
{
    public class ArenaDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        
        public List<CourtDetailsDTO> CourtDetails { get; set; } = new List<CourtDetailsDTO>();
    }
}
