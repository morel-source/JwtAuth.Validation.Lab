using Analytics.Worker.Monitoring;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Analytics.Worker.Consumers;

public sealed class DeviceAuthenticatedConsumer(
    ILogger<DeviceAuthenticatedConsumer> logger,
    IMetricsService metricsService
) : IConsumer<DeviceAuthenticated>
{
    public Task Consume(ConsumeContext<DeviceAuthenticated> context)
    {
        logger.LogDebug("Received DeviceAuthenticated event for {Id}", context.Message.DeviceId);
        metricsService.IncrementAuthSuccess();
        return Task.CompletedTask;
    }
}