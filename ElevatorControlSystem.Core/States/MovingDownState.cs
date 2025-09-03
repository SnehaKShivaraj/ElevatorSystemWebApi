using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class MovingDownState : IElevatorState
{
    public void HandleRequest(Elevator elevator)
    {
      if (elevator.HasDownRequests && elevator.CurrentFloor > elevator.MinFloor)
        {
            // Already moving up, do nothing
            return;
        }
        if (elevator.HasUpRequests)
        {
            // No more up requests, switch to moving down
            elevator.ChangeState(new MovingUpState(), EDirection.Up);
        }
        else
        {
            // No requests, become idle
            elevator.ChangeState(new IdleState(), EDirection.Idle);
        }
    }

    public async Task Move(Elevator elevator)
    {
        if (!elevator.HasDownRequests)
    {
        HandleRequest(elevator);
        return;
    }

    var target = elevator.DownRequests.Max;  // largest floor request

    if (target < elevator.CurrentFloor)
    {
        elevator.DecrementFloor();
        await Task.Delay(elevator.TimeToTravelOneFloorInMilliSeconds);
    }

    if (elevator.CurrentFloor == target && elevator.TryRemoveDownRequestFloor(target))
    {
        await Task.Delay(elevator.TimeOnFloorInMilliSeconds);
    }
    if(!elevator.HasDownRequests)
        HandleRequest(elevator);

    }

}
