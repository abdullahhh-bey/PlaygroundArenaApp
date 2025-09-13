using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddTimeSlotsByCourtIdDTO
    {
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int CourtId { get; set; }
    }
}
