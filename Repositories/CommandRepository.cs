using Microsoft.EntityFrameworkCore;
using RobotVacuumWebAPI.Data;
using RobotVacuumWebAPI.Interfaces;
using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly RobotVacuumDbContext _context;

        public CommandRepository(RobotVacuumDbContext context)
        {
            _context = context;
        }

        public async Task<Command?> GetPendingCommandAsync(int robotId)
        {
            return await _context.Commands
                .Where(c => c.RobotVacuumId == robotId && c.Status == "Pending")
                .OrderBy(c => c.ScheduledTime)
                .FirstOrDefaultAsync();
        }

        public async Task AddCommandAsync(Command command)
        {
            await _context.Commands.AddAsync(command);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCommandStatusAsync(int commandId, string status)
        {
            var command = await _context.Commands.FindAsync(commandId);
            if (command != null)
            {
                command.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Command>> GetCommandHistoryAsync(int robotId, int days)
        {
            var dateThreshold = DateTime.UtcNow.AddDays(-days);
            return await _context.Commands
                .Where(c => c.RobotVacuumId == robotId && c.ScheduledTime >= dateThreshold)
                .OrderByDescending(c => c.ScheduledTime)
                .ToListAsync();
        }
    }
}