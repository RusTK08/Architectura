namespace RobotVacuumWebAPI.Models
{
    public class CleaningSession
    {
        public int Id { get; set; }
        public int RobotVacuumId { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public decimal AreaCleaned { get; set; }
        public string MapData { get; set; } = string.Empty;
    }
}
