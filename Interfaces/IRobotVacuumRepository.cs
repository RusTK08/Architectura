using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Interfaces
{
    public interface IRobotVacuumRepository
    {
        Task<RobotVacuum?> GetByIdAsync(int id);
        Task<List<RobotVacuum>> GetAllAsync();
        Task AddAsync(RobotVacuum robot);
        Task UpdateAsync(RobotVacuum robot);
        Task DeleteAsync(int id);
    }
}
