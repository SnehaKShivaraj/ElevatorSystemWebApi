using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class IdleState : IElevatorState
{
    public void HandleRequest(Elevator elevator, int floor)
    {
        if (elevator.HasUpRequests)
        {
            elevator.ChangeState(new MovingUpState(), EDirection.Up);
            return;
        }
        if (elevator.HasDownRequests)
        {
            elevator.ChangeState(new MovingDownState(), EDirection.Down);
        }
    }

    public Task Move(Elevator elevator)
    {
        // Idle does nothing
        return Task.CompletedTask;
    }
}
