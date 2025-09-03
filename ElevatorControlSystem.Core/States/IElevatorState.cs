using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public interface IElevatorState
{
    void HandleRequest(Elevator elevator);
    Task Move(Elevator elevator); 
}
