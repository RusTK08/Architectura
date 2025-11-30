using System.ComponentModel.DataAnnotations;

namespace RobotVacuumWebAPI.Models
{
    public class Command
    {
        public int Id { get; set; }
        public int RobotVacuumId { get; set; }
        
        [Required]
        public string CommandType { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
        public DateTime ScheduledTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
    }
}
