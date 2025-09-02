
using ElevatorControlSystem.Core;
using ElevatorControlSystem.Core.Dtos;
using ElevatorControlSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/elevators")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly IElevatorService _elevatorService;

        public ElevatorController(IElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }

        [HttpGet("settings")]
        public ActionResult<ElevatorSettings> GetElevatorSettings()
        {
            return Ok(_elevatorService.GetElevatorSettings());
        }

        [HttpGet("")]
        public ActionResult<IEnumerable<Elevator>> GetElevators()
        {
            return Ok(_elevatorService.GetElevatorsDetailsState());
        }

        [HttpPost("call")]
        public IActionResult CallElevator([FromBody] CallRequestDto request)
        {

            var response = _elevatorService.AddExternalRequestAsync(request.Floor, request.Direction);
            return Ok(response);
        }
        [HttpPost("press")]
        public IActionResult PressButton([FromBody] PressRequestDto request)
        {
            var response = _elevatorService.AddInternalRequestAsync(request.ElevatorId, request.Floor);
            return Ok(response);
        }
    }
}
