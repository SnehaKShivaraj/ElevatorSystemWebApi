using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class MovingUpState : IElevatorState
{
    public void HandleRequest(Elevator elevator)
    {
        if (elevator.HasUpRequests && elevator.CurrentFloor < elevator.MaxFloor)
        {
            // Already moving up, do nothing
            return;
        }
        if (elevator.HasDownRequests)
        {
            // No more up requests, switch to moving down
            elevator.ChangeState(new MovingDownState(), EDirection.Down);
        }
        else
        {
            // No requests, become idle
            elevator.ChangeState(new IdleState(), EDirection.Idle);
        }
    }

    public async Task Move(Elevator elevator)
    {
        if (!elevator.HasUpRequests)
        {
            HandleRequest(elevator);
            return;
        }

        var target = elevator.UpRequests.Min;  // smallest floor request

        if (target > elevator.CurrentFloor)
        {
            elevator.IncrementFloor();
            await Task.Delay(elevator.TimeToTravelOneFloorInMilliSeconds);
        }

        if (elevator.CurrentFloor == target && elevator.TryRemoveUpRequestFloor(target))
        {
            await Task.Delay(elevator.TimeOnFloorInMilliSeconds);
        }

        // After one step, re-evaluate
    if(!elevator.HasUpRequests)
        HandleRequest(elevator);
    }

}
