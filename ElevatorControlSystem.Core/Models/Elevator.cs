

namespace ElevatorControlSystem.Core.Models;

public class Elevator
{
    public int Id { get; set; }
    public int CurrentFloor { get; private set; }
    public int MaxFloor { get; set; } = 10;
    public int MinFloor { get; set; } = 1;
    public int TimeOnFloorInMilliSeconds { get; set; }
    public int TimeToTravelOneFloorInMilliSeconds { get; set; }
    public EDirection Direction { get; private set; }
    public bool IsMoving => Direction != EDirection.Idle;
    public HashSet<int> UpRequests { get; private set; }
    public HashSet<int> DownRequests { get; private set; }
    public IElevatorState CurrentState { get; private set; }
    private CancellationTokenSource cts = new();
    private readonly object _lock = new object();

    public Elevator(int id, ElevatorSettings elevatorSettings)
    {
        Id = id;
        CurrentFloor = 1;
        TimeOnFloorInMilliSeconds = elevatorSettings.TimeOnFloorInSeconds * 1000;
        TimeToTravelOneFloorInMilliSeconds = elevatorSettings.TimeToTravelOneFloorInSeconds * 1000;
        MaxFloor = elevatorSettings.MaxFloor;
        MinFloor = elevatorSettings.MinFloor;
        UpRequests = new HashSet<int>();
        DownRequests = new HashSet<int>();
        this.CurrentState = new IdleState();
    }
    public bool HasUpRequests => UpRequests.Count > 0;
    public bool HasDownRequests => DownRequests.Count > 0;
    private void AddUpRequest(int floor)
    {
        lock (_lock)
        {
            UpRequests.Add(floor);

        }
    }
    private void AddDownRequest(int floor)
    {
        lock (_lock)
        {
            DownRequests.Add(floor);

        }
    }
    public void AddFloorRequest(int floor)
    {
        if (CurrentFloor == floor)
        {
            return;
        }

        if (floor > CurrentFloor)
        {
            AddUpRequest(floor);
        }
        else
        {
            AddDownRequest(floor);
        }

        CurrentState.HandleRequest(this);
    }
    public void AddFloorRequest(int floor, EDirection direction)
    {
        if (direction == EDirection.Idle)
        {
            return;
        }

        if (direction == EDirection.Up)
        {
            AddUpRequest(floor);
        }
        else
        {
            AddDownRequest(floor);

        }
        CurrentState.HandleRequest(this);
    }
    public void IncrementFloor()
    {
        if (CurrentFloor < MaxFloor) CurrentFloor++;
    }

    public void DecrementFloor()
    {
        if (CurrentFloor > MinFloor) CurrentFloor--;
    }

    public bool TryRemoveUpRequestFloor(int floor)
    {

        bool removed = false;
        if (!HasUpRequests) return removed;
        lock (_lock)
        {
            UpRequests.Remove(floor);
            removed = true;
        }
        return removed;
    }
    public bool TryRemoveDownRequestFloor(int floor)
    {
        bool removed = false;
        if (!HasDownRequests) return removed;
        lock (_lock)
        {
            DownRequests.Remove(floor);
            removed = true;
        }
        return removed;
    }

    public void ChangeState(IElevatorState newState, EDirection newDirection)
    {
        if (newState != null)
        {
            CurrentState = newState;
            this.Direction = newDirection;
        }
    }

    public void Stop()
    {
        cts.Cancel();
    }
    public void Start()
    {
        Task.Run(async () =>
        {
            while (!cts.IsCancellationRequested)
            {
                await CurrentState.Move(this);
                await Task.Delay(500, cts.Token); // short pause
            }
        }, cts.Token);
    }

}
