using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Interfaces
{
    public interface ICommandRepository
    {
        Task<Command?> GetPendingCommandAsync(int robotId);
        Task AddCommandAsync(Command command);
        Task UpdateCommandStatusAsync(int commandId, string status);
        Task<List<Command>> GetCommandHistoryAsync(int robotId, int days);
    }
}
