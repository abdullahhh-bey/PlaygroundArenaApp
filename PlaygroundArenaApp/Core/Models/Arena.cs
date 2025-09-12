using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.Models
{
    public class Arena
    {
        public int ArenaId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<Court> Courts { get; set; } = new List<Court>();
    }
}
