using Microsoft.EntityFrameworkCore;
using RobotVacuumWebAPI.Data;
using RobotVacuumWebAPI.Interfaces;
using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Repositories
{
    public class RobotVacuumRepository : IRobotVacuumRepository
    {
        private readonly RobotVacuumDbContext _context;

        public RobotVacuumRepository(RobotVacuumDbContext context)
        {
            _context = context;
        }

        public async Task<RobotVacuum?> GetByIdAsync(int id)
        {
            return await _context.RobotVacuums.FindAsync(id);
        }

        public async Task<List<RobotVacuum>> GetAllAsync()
        {
            return await _context.RobotVacuums.ToListAsync();
        }

        public async Task AddAsync(RobotVacuum robot)
        {
            await _context.RobotVacuums.AddAsync(robot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RobotVacuum robot)
        {
            _context.RobotVacuums.Update(robot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var robot = await GetByIdAsync(id);
            if (robot != null)
            {
                _context.RobotVacuums.Remove(robot);
                await _context.SaveChangesAsync();
            }
        }
    }
}