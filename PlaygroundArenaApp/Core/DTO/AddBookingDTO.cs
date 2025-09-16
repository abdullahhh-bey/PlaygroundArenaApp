using PlaygroundArenaApp.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.DTO
{
    public class AddBookingDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CourtId { get; set; }
        [Required]
        public List<int> TimeSlotId { get; set; } = new List<int>();
        [Required]
        public string BookingStatus { get; set; }
    }
}
