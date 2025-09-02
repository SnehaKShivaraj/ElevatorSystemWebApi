namespace ElevatorControlSystem.Core.Models;

public class Building
{
    public int TotalFloors { get; set; }
    public IList<Elevator> Elevators { get; private set; }
    public Building(ElevatorSettings elevatorSettings)
    {
        TotalFloors = elevatorSettings.NumberOfFloors;
        Elevators = Enumerable.Range(1, elevatorSettings.NumberOfElevators).
        Select(id => new Elevator(id, elevatorSettings))
        .ToList();

    }

}