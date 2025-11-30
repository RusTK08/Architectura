using Microsoft.EntityFrameworkCore;
using RobotVacuumWebAPI.Data;
using RobotVacuumWebAPI.Interfaces;
using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Services
{
    public class RobotVacuumService : IRobotVacuumService
    {
        private readonly IRobotVacuumRepository _robotRepository;
        private readonly ICommandRepository _commandRepository;
        private readonly RobotVacuumDbContext _context;
        private readonly ILogger<RobotVacuumService> _logger;
        public RobotVacuumService(
            IRobotVacuumRepository robotRepository,
            ICommandRepository commandRepository,
            RobotVacuumDbContext context,
            ILogger<RobotVacuumService> logger)
        {
            _robotRepository = robotRepository;
            _commandRepository = commandRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<RobotVacuum?> GetRobotStatusAsync(int robotId)
        {
            return await _robotRepository.GetByIdAsync(robotId);
        }

        public async Task SendCommandAsync(int robotId, string commandType, string parameters)
        {
            var command = new Command
            {
                RobotVacuumId = robotId,
                CommandType = commandType,
                Parameters = parameters,
                ScheduledTime = DateTime.UtcNow,
                Status = "Pending"
            };

            await _commandRepository.AddCommandAsync(command);
            _logger.LogInformation("Command {CommandType} sent to robot {RobotId}", commandType, robotId);
        }

        public async Task<string> ExecutePendingCommandAsync(int robotId)
        {
            var pendingCommand = await _commandRepository.GetPendingCommandAsync(robotId);
            if (pendingCommand == null)
                return "No pending commands";

            try
            {
                var robot = await _robotRepository.GetByIdAsync(robotId);
                if (robot == null)
                    return "Robot not found";

                robot.Status = GetRobotStatusForCommand(pendingCommand.CommandType);
                robot.BatteryLevel = Math.Max(0, robot.BatteryLevel - 10); // Симуляция расхода батареи

                await _robotRepository.UpdateAsync(robot);
                await _commandRepository.UpdateCommandStatusAsync(pendingCommand.Id, "Executed");

                return $"Command {pendingCommand.CommandType} executed successfully";
            }
            catch (Exception ex)
            {
                await _commandRepository.UpdateCommandStatusAsync(pendingCommand.Id, "Failed");
                _logger.LogError(ex, "Failed to execute command for robot {RobotId}", robotId);
                return "Command execution failed";
            }
        }

        public async Task<List<CleaningSession>> GetCleaningHistoryAsync(int robotId)
        {
            return await _context.CleaningSessions
                .Where(c => c.RobotVacuumId == robotId)
                .OrderByDescending(c => c.StartTime)
                .ToListAsync();
        }

        public async Task<RobotVacuum> CreateRobotAsync(string name)
        {
            var robot = new RobotVacuum
            {
                Name = name,
                Status = "Idle",
                BatteryLevel = 100,
                LastCleaningTime = DateTime.UtcNow,
                CurrentLocation = "Home"
            };

            await _robotRepository.AddAsync(robot);
            return robot;
        }

        private static string GetRobotStatusForCommand(string commandType)
        {
            return commandType switch
            {
                "StartCleaning" => "Cleaning",
                "Stop" => "Idle",
                "ReturnToBase" => "Charging",
                "SpotClean" => "Spot Cleaning",
                _ => "Idle"
            };
        }
    }
}