using RobotVacuumWebAPI.Models;
namespace RobotVacuumWebAPI.Interfaces
{
    public interface IRobotVacuumService
    {
        Task<RobotVacuum?> GetRobotStatusAsync(int robotId);
        Task SendCommandAsync(int robotId, string commandType, string parameters);
        Task<List<CleaningSession>> GetCleaningHistoryAsync(int robotId);
        Task<string> ExecutePendingCommandAsync(int robotId);
        Task<RobotVacuum> CreateRobotAsync(string name);
    }
}