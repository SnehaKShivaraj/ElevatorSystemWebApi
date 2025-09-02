using ElevatorControlSystem.Core.Dtos;
using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public interface IElevatorService
{
    ElevatorSettings GetElevatorSettings();
    IEnumerable<ElevatorDetailsResponseDto> GetElevatorsDetailsState(); 
    CallElevatorResponseDto AddExternalRequestAsync(int floor, EDirection direction);
    CallElevatorResponseDto AddInternalRequestAsync(int elevatorId, int floor);
}
