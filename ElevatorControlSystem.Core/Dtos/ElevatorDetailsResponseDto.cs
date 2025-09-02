
using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core.Dtos;
public class ElevatorDetailsResponseDto
{
    public int ElevatorId { get; set; }
    public int CurrentFloor { get; set; }
    public EDirection Direction { get; set; }
    public bool IsIdle { get; set; }
    public IList<int> UpRequests { get; set; } = new List<int>();
    public IList<int> DownRequests { get; set; } = new List<int>();
}
