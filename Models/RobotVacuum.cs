using System.ComponentModel.DataAnnotations;

namespace RobotVacuumWebAPI.Models
{
    public class RobotVacuum
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Idle";
        public int BatteryLevel { get; set; } = 100;
        public DateTime LastCleaningTime { get; set; } = DateTime.UtcNow;
        public string CurrentLocation { get; set; } = "Home";
    }
}
