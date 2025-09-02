namespace ElevatorControlSystem.Core.Dtos;

public enum EElevatorQueueError
{
    InvalidFloorError = 1,
    InvalidDirectionError = 2,
    InvalidElevatorId = 3,
    ElevatorIsAlreaadyServing = 4,
    ElevatorRequestQueingFailure = 5
}