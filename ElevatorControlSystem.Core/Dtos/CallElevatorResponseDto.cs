
namespace ElevatorControlSystem.Core.Dtos;

public class CallElevatorResponseDto
{
    public IList<EElevatorQueueError> Errors { get; set; }
    public string SuccessMessage { get; set; }
}