using Microsoft.AspNetCore.Mvc;
using RobotVacuumWebAPI.Interfaces;
using RobotVacuumWebAPI.Models;

namespace RobotVacuumWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RobotVacuumController : ControllerBase
    {
        private readonly IRobotVacuumService _robotService;
        private readonly ILogger<RobotVacuumController> _logger;
        public RobotVacuumController(IRobotVacuumService robotService, ILogger<RobotVacuumController> logger)
        {
            _robotService = robotService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RobotVacuum>> GetRobotStatus(int id)
        {
            var robot = await _robotService.GetRobotStatusAsync(id);
            if (robot == null)
                return NotFound();

            return Ok(robot);
        }

        [HttpPost]
        public async Task<ActionResult<RobotVacuum>> CreateRobot([FromBody] CreateRobotRequest request)
        {
            try
            {
                var robot = await _robotService.CreateRobotAsync(request.Name);
                return CreatedAtAction(nameof(GetRobotStatus), new { id = robot.Id }, robot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating robot");
                return BadRequest(new { Error = "Failed to create robot" });
            }
        }

        [HttpPost("{id}/commands")]
        public async Task<ActionResult> SendCommand(int id, [FromBody] CommandRequest request)
        {
            try
            {
                await _robotService.SendCommandAsync(id, request.CommandType, request.Parameters);
                return Ok(new { Message = "Command sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending command to robot {RobotId}", id);
                return BadRequest(new { Error = "Failed to send command" });
            }
        }

        [HttpPost("{id}/execute")]
        public async Task<ActionResult<string>> ExecuteCommand(int id)
        {
            var result = await _robotService.ExecutePendingCommandAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<List<CleaningSession>>> GetCleaningHistory(int id)
        {
            var history = await _robotService.GetCleaningHistoryAsync(id);
            return Ok(history);
        }

        [HttpGet]
        public async Task<ActionResult<List<RobotVacuum>>> GetAllRobots()
        {
            // Для простоты добавим прямой доступ к репозиторию
            return Ok("Use specific endpoints for robot operations");
        }
    }

    public class CreateRobotRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CommandRequest
    {
        public string CommandType { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;
    }
}