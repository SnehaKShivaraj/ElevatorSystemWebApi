using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core;

public class MovingUpState : IElevatorState
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
        while (elevator.HasUpRequests && elevator.CurrentFloor < elevator.MaxFloor)
        {
            elevator.IncrementFloor();
            await Task.Delay(elevator.TimeToTravelOneFloorInMilliSeconds); // better: async instead of Thread.Sleep

            if (elevator.TryRemoveUpRequestFloor(elevator.CurrentFloor))
            {
                await Task.Delay(elevator.TimeOnFloorInMilliSeconds);
            }
        }

        if (elevator.HasDownRequests)
            {
                elevator.ChangeState(new MovingDownState(), EDirection.Down);
            }
            else
            {
                elevator.ChangeState(new IdleState(), EDirection.Idle);
            }
    }

}
