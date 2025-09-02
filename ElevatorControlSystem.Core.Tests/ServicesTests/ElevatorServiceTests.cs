using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using ElevatorControlSystem.Core.Models;
using ElevatorControlSystem.Core.Dtos;

namespace ElevatorControlSystem.Core.Tests;


public class ElevatorServiceTests
{
    private readonly Mock<ILogger<ElevatorService>> _loggerMock;
    private readonly ElevatorService _service;
    private readonly ElevatorSettings _settings;

    public ElevatorServiceTests()
    {
        _loggerMock = new Mock<ILogger<ElevatorService>>();

        _settings = new ElevatorSettings
        {
            MinFloor = 1,
            MaxFloor = 10,
            NumberOfElevators = 4,
            NumberOfFloors = 10
        };

        _service = new ElevatorService(_loggerMock.Object, _settings);
    }

    [Fact]
    public void AddExternalRequest_InvalidFloor_ReturnsError()
    {
        // Arrange
        int invalidFloor = 20;

        // Act
        var result = _service.AddExternalRequestAsync(invalidFloor, EDirection.Up);

        // Assert
        result.Errors.Should().Contain(EElevatorQueueError.InvalidFloorError);
        result.SuccessMessage.Should().BeNull();
    }

    [Fact]
    public void AddExternalRequest_ValidFloor_AssignsElevator()
    {
        int floor = 5;

        var result = _service.AddExternalRequestAsync(floor, EDirection.Up);

        result.Errors.Should().BeEmpty();
        result.SuccessMessage.Should().Contain("Elevator");

        // Elevator should have the floor in requests
        var elevator = _service.GetElevatorsDetailsState().First(e => e.ElevatorId == 1);
        elevator.UpRequests.Should().Contain(floor);
    }

    [Fact]
    public void AddInternalRequest_InvalidElevatorId_ReturnsError()
    {
        int invalidElevatorId = 10;

        var result = _service.AddInternalRequestAsync(invalidElevatorId, 3);

        result.Errors.Should().Contain(EElevatorQueueError.InvalidElevatorId);
    }

    [Fact]
    public void AddInternalRequest_ValidRequest_AddsToElevator()
    {
        int elevatorId = 2;
        int floor = 7;

        var result = _service.AddInternalRequestAsync(elevatorId, floor);

        result.Errors.Should().BeEmpty();
        result.SuccessMessage.Should().Contain($"Elevator {elevatorId}");

        var elevator = _service.GetElevatorsDetailsState().First(e => e.ElevatorId == elevatorId);
        elevator.UpRequests.Should().Contain(floor);
    }

    [Fact]
    public void SelectElevator_ReturnsNearestElevator()
    {
        // Elevator 1 is at floor 1, Elevator 2 at floor 1 initially
        // Request at floor 3 → nearest is elevator 1 (both equal, picks first)
        var result = _service.AddExternalRequestAsync(3, EDirection.Up);
        var assignedElevator = _service.GetElevatorsDetailsState().First(e => e.UpRequests.Contains(3));

        assignedElevator.CurrentFloor.Should().Be(1); // nearest floor
    }
}
