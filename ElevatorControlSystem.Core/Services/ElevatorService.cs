using ElevatorControlSystem.Core.Dtos;
using ElevatorControlSystem.Core.Models;
using Microsoft.Extensions.Logging;

namespace ElevatorControlSystem.Core;

public class ElevatorService : IElevatorService
{
    private readonly ElevatorSettings _elevatorSettings;
    private readonly IList<Elevator> _elevators;
    private readonly ILogger<ElevatorService> _logger;


    public ElevatorService(ILogger<ElevatorService> logger, ElevatorSettings elevatorSettings)
    {
        _elevators = new Building(elevatorSettings).Elevators;
        this._logger = logger;
        this._logger.LogInformation("Building initialized: {Floors} floors, {Elevators} elevators", elevatorSettings.NumberOfFloors, elevatorSettings.NumberOfElevators);
        this._elevatorSettings = elevatorSettings;
        StartElevators();

    }
    private void StartElevators()
    {
        if (_elevators == null)
        {
            return;
        }
        foreach (var elevator in _elevators)
        {
            _logger.LogInformation($"Starting the elevator - {elevator.Id}");
            //Start(elevator);
            elevator.Start();
            _logger.LogInformation($"Completed the elevator - {elevator.Id}");
        }
    }

    public ElevatorSettings GetElevatorSettings()
    {
        // Simply return the current snapshot of building
        return _elevatorSettings;
    }
    public IEnumerable<ElevatorDetailsResponseDto> GetElevatorsDetailsState()
    {
        // Simply return the current snapshot of building
        return _elevators.Select(elevator => new ElevatorDetailsResponseDto
        {
            ElevatorId = elevator.Id,
            CurrentFloor = elevator.CurrentFloor,
            Direction = elevator.Direction,
            IsIdle = elevator.Direction == EDirection.Idle,
            UpRequests = elevator.UpRequests.ToList(),
            DownRequests = elevator.DownRequests.ToList()
        });
    }
    public CallElevatorResponseDto AddExternalRequestAsync(int floor, EDirection direction)
    {
        CallElevatorResponseDto response = new CallElevatorResponseDto();
        _logger.LogInformation("Received external request: Floor {Floor}, Direction {Direction}", floor, direction);

        // Choose an elevator (simple strategy for now → first idle elevator)
        response.Errors = Validate(floor, direction);
        if (response.Errors.Count > 0)
        {
            return response;
        }
        if (_elevators.Any(e => (e.UpRequests.Contains(floor) && e.Direction == direction)
        || (e.DownRequests.Contains(floor) && e.Direction == direction)))
        {
            response.Errors = new List<EElevatorQueueError> { EElevatorQueueError.ElevatorIsAlreaadyServing };
            return response;
        }
        var elevator = SelectElevator(floor, direction);

        if (elevator == null)
        {
            // fallback → pick the first elevator for now
            elevator = _elevators.First();
        }

        elevator.AddFloorRequest(floor, direction);
        response.SuccessMessage = $"Request assigned to Elevator {elevator.Id}";
        _logger.LogInformation(response.SuccessMessage);
        return response;

    }
    public CallElevatorResponseDto AddInternalRequestAsync(int elevatorId, int floor)
    {
        CallElevatorResponseDto response = new CallElevatorResponseDto();

        _logger.LogInformation("Received internal request: Floor {Floor}, Elevator {ElevatorId}", floor, elevatorId);
        response.Errors = Validate(floor, elevatorId: elevatorId);
        if (response.Errors.Count > 0)
        {
            return response;
        }
        var elevator = _elevators
            .First(e => e.Id == elevatorId);

        elevator.AddFloorRequest(floor);
        response.SuccessMessage = $"Request assigned to Elevator {elevator.Id}";
        _logger.LogInformation(response.SuccessMessage);
        return response;
    }
    private IList<EElevatorQueueError> Validate(int floor, EDirection? direction = default, int? elevatorId = default)
    {
        var errorList = new List<EElevatorQueueError>();
        if (floor > _elevatorSettings.MaxFloor || floor < _elevatorSettings.MinFloor)
        {
            errorList.Add(EElevatorQueueError.InvalidFloorError);
        }
        if (direction.HasValue && !new[] { EDirection.Up, EDirection.Down }.Contains(direction.Value))
        {
            errorList.Add(EElevatorQueueError.InvalidDirectionError);
        }
        if (elevatorId.HasValue && (elevatorId.Value > _elevatorSettings.NumberOfElevators || elevatorId.Value < 1))
        {
            errorList.Add(EElevatorQueueError.InvalidElevatorId);
        }
        return errorList;
    }


    private Elevator? SelectElevator(int floor, EDirection direction)
    {
        // 1️⃣ Elevators moving in the requested direction and can serve the request
        var movingElevator = _elevators
            .Where(e => e.Direction == direction)
            .Where(e => (direction == EDirection.Up && e.CurrentFloor <= floor) ||
                        (direction == EDirection.Down && e.CurrentFloor >= floor))
            .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
            .ThenBy(e => e.UpRequests.Count + e.DownRequests.Count) // load balancing
            .FirstOrDefault();

        if (movingElevator != null)
        {
            _logger.LogInformation($"Found moving elevator - {movingElevator.Id} for request {floor} - direction {direction}");
            return movingElevator;
        }

        // 2️⃣ Idle elevators
        var idleElevator = _elevators
            .Where(e => e.Direction == EDirection.Idle)
            .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
            .ThenBy(e => e.UpRequests.Count + e.DownRequests.Count) // load balancing
            .FirstOrDefault();

        if (idleElevator != null)
        {
            _logger.LogInformation($"Found idle elevator - {idleElevator.Id} for request {floor} - direction {direction}");
            return idleElevator;
        }

        // 3️⃣ Nearest elevator regardless of direction (even if moving opposite)
        var nearestElevator = _elevators
            .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
            .ThenBy(e => e.UpRequests.Count + e.DownRequests.Count) // load balancing
            .FirstOrDefault();

        if (nearestElevator != null)
        {
            _logger.LogInformation($"Found nearest elevator - {nearestElevator.Id} for request {floor} - direction {direction}");
        }

        return nearestElevator;
    }

    public IEnumerable<Elevator> GetElevators()
    {
        return _elevators;
    }
}
