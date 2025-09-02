using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public interface IElevatorState
{
    void HandleRequest(Elevator elevator, int floor);
    Task Move(Elevator elevator); 
}
