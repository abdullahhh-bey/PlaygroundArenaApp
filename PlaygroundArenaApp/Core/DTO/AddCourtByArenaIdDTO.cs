using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddCourtByArenaIdDTO
    {
        [Required]
        public int ArenaId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string CourtType { get; set; } = string.Empty;

    }
}
