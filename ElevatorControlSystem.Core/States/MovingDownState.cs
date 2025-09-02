using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class MovingDownState : IElevatorState
{
    public void HandleRequest(Elevator elevator, int floor)
    {
        if (elevator.CurrentFloor == floor)
        {
            return;
        }
        elevator.AddFloorRequest(floor);
    }

    public async Task Move(Elevator elevator)
    {
        while (elevator.DownRequests.Count > 0 && elevator.CurrentFloor > elevator.MinFloor)
        {
            elevator.DecrementFloor();
            await Task.Delay(elevator.TimeToTravelOneFloorInMilliSeconds);
            if (elevator.TryRemoveDownRequestFloor(elevator.CurrentFloor))
            {
                await Task.Delay(elevator.TimeOnFloorInMilliSeconds);
            }
        }
        if (elevator.HasUpRequests)
            {
                elevator.ChangeState(new MovingUpState(), EDirection.Up);
            }
            else
            {
                elevator.ChangeState(new IdleState(), EDirection.Idle);
            }
    }
    
}
