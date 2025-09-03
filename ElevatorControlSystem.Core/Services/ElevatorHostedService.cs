using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Core
{
    public class ElevatorHostedService : IHostedService
    {
        private readonly IElevatorService _elevatorService;
        private readonly ILogger<ElevatorHostedService> _logger;

        public ElevatorHostedService(IElevatorService elevatorService, ILogger<ElevatorHostedService> logger)
        {
            _elevatorService = elevatorService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ElevatorHostedService starting...");

            var elevators = _elevatorService.GetElevators();
            foreach (var elevator in elevators)
            {
                elevator.Start();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ElevatorHostedService stopping...");

            // TODO: stop elevators here
            
            var elevators = _elevatorService.GetElevators();
            foreach (var elevator in elevators)
            {
                elevator.Stop();
            }

            return Task.CompletedTask;
        }
    }
}
