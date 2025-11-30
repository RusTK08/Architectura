using Microsoft.EntityFrameworkCore;
using RobotVacuumWebAPI.Data;
using RobotVacuumWebAPI.Interfaces;
using RobotVacuumWebAPI.Repositories;
using RobotVacuumWebAPI.Services;
using RobotVacuumWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
builder.Services.AddDbContext<RobotVacuumDbContext>(options =>
    options.UseSqlite("Data Source=robot_vacuum.db"));

// Dependency Injection
builder.Services.AddScoped<IRobotVacuumRepository, RobotVacuumRepository>();
builder.Services.AddScoped<ICommandRepository, CommandRepository>();
builder.Services.AddScoped<IRobotVacuumService, RobotVacuumService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize database with test data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RobotVacuumDbContext>();
    dbContext.Database.EnsureCreated();
    
    // Добавляем тестового робота если база пустая
    if (!dbContext.RobotVacuums.Any())
    {
        dbContext.RobotVacuums.Add(new RobotVacuum
        { 
            Name = "Super Cleaner 3000", 
            Status = "Idle", 
            BatteryLevel = 100,
            CurrentLocation = "Living Room"
        });
        dbContext.SaveChanges(); // Убрать await для .NET 6
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();