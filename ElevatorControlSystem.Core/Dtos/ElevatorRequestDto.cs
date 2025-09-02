using ElevatorControlSystem.Core.Models;

namespace ElevatorControlSystem.Core.Dtos;

public class ElevatorRequestDto
{
        public int SourceFloor { get; set; }
        public int DestinationFloor { get; set; }
        public EDirection Direction { get; set; } // "Up" or "Down"
        public int? ElevatorId { get; set; } 
}
