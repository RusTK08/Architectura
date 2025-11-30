using Microsoft.EntityFrameworkCore;
using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Data
{
    public class RobotVacuumDbContext : DbContext
    {
        public RobotVacuumDbContext(DbContextOptions<RobotVacuumDbContext> options) : base(options) { }

        public DbSet<RobotVacuum> RobotVacuums => Set<RobotVacuum>();
        public DbSet<Command> Commands => Set<Command>();
        public DbSet<CleaningSession> CleaningSessions => Set<CleaningSession>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RobotVacuum>().ToTable("RobotVacuums");
            modelBuilder.Entity<Command>().ToTable("Commands");
            modelBuilder.Entity<CleaningSession>().ToTable("CleaningSessions");
        }
    }
}
