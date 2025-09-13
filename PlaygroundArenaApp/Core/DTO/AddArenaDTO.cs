using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddArenaDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Location {  get; set; } = string.Empty; 
    }
}
