using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ElevatorControlSystem.Core;
using ElevatorControlSystem.Core.Models;
using ElevatorControlSystem.Core.Dtos;
using MyApp.Namespace; // your controller namespace
using System.Collections.Generic;

namespace ElevatorControlSystem.Api.Tests
{
    public class ElevatorControllerTests
    {
        private readonly Mock<IElevatorService> _serviceMock;
        private readonly ElevatorController _controller;

        public ElevatorControllerTests()
        {
            _serviceMock = new Mock<IElevatorService>();
            _controller = new ElevatorController(_serviceMock.Object);
        }

        [Fact]
        public void GetElevatorSettings_ReturnsSettings()
        {
            // Arrange
            var settings = new ElevatorSettings { MinFloor = 1, MaxFloor = 10, NumberOfElevators = 4, NumberOfFloors = 10 };
            _serviceMock.Setup(s => s.GetElevatorSettings()).Returns(settings);

            // Act
            var result = _controller.GetElevatorSettings();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(settings);
        }

        [Fact]
        public void GetElevators_ReturnsElevatorList()
        {
            var elevators = new List<ElevatorDetailsResponseDto>
            {
                new ElevatorDetailsResponseDto { ElevatorId = 1, CurrentFloor = 1, Direction = EDirection.Idle },
                new ElevatorDetailsResponseDto { ElevatorId = 2, CurrentFloor = 1, Direction = EDirection.Idle }
            };
            _serviceMock.Setup(s => s.GetElevatorsDetailsState()).Returns(elevators);

            var result = _controller.GetElevators();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(elevators);
        }

        [Fact]
        public void CallElevator_ValidRequest_ReturnsResponse()
        {
            var request = new CallRequestDto { Floor = 5, Direction = EDirection.Up };
            var responseDto = new CallElevatorResponseDto { SuccessMessage = "Assigned to elevator 1" };

            _serviceMock.Setup(s => s.AddExternalRequestAsync(request.Floor, request.Direction)).Returns(responseDto);

            var result = _controller.CallElevator(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(responseDto);
        }

        [Fact]
        public void PressButton_ValidRequest_ReturnsResponse()
        {
            var request = new PressRequestDto { ElevatorId = 2, Floor = 7 };
            var responseDto = new CallElevatorResponseDto { SuccessMessage = "Assigned to elevator 2" };

            _serviceMock.Setup(s => s.AddInternalRequestAsync(request.ElevatorId, request.Floor)).Returns(responseDto);

            var result = _controller.PressButton(request);

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(responseDto);
        }
    }
}
