using System.ComponentModel.DataAnnotations;

namespace PlaygroundArenaApp.Core.Models
{
    public class CourtRules
    {
        [Key]
        public int RuleId { get; set; }
        public int CourtId { get ; set; }
        public int TimeInterval { get; set; }
        public int MinimumSlotsBooking { get; set; }
    }
}
