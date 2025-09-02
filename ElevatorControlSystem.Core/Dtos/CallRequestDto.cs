using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core.Dtos;

public class CallRequestDto
{
    public int Floor { get; set; }
    public EDirection Direction { get; set; } 
}
