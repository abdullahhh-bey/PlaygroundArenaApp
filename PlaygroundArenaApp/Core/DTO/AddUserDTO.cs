using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddUserDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
