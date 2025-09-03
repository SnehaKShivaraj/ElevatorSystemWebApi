using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class IdleState : IElevatorState
{
    public void HandleRequest(Elevator elevator)
    {
        if (elevator.HasUpRequests)
        {
            // Move up if there are up requests
            elevator.ChangeState(new MovingUpState(), EDirection.Up);
        }
        else if (elevator.HasDownRequests)
        {
            // Move down if there are down requests
            elevator.ChangeState(new MovingDownState(), EDirection.Down);
        }
    }

    public Task Move(Elevator elevator)
    {
        // Idle does nothing
        HandleRequest(elevator);
        return Task.CompletedTask;
    }
}
