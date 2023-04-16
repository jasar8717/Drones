using Drones.Core.Services;
using Drones.Services;

namespace Drones.Api.Resource
{
    public class PeriodicDroneCheckin : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromHours(6);
        private readonly ILogger<PeriodicDroneCheckin> _logger;
        private readonly IServiceScopeFactory _factory;
        private int _executionCount = 0;

        public PeriodicDroneCheckin(ILogger<PeriodicDroneCheckin> logger, IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

                    IDroneService sampleService = asyncScope.ServiceProvider.GetRequiredService<IDroneService>();
                    var batteryLevel = sampleService.CheckBatteryLevel(stoppingToken);

                    _executionCount++;
                    _logger.LogInformation($"Executed Periodic Drone Battery Check Level - Count: {_executionCount} - {batteryLevel}.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Failed to execute PeriodicHostedService with exception message {ex.Message}.");
                }
            }
        }
    }
}
