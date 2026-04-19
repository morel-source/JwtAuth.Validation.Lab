using Analytics.Worker.Monitoring;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Analytics.Worker.Consumers;

public sealed class DeviceAuthFailedConsumer(
    ILogger<DeviceAuthFailedConsumer> logger,
    IMetricsService metricsService
) : IConsumer<DeviceAuthFailed>
{
    public Task Consume(ConsumeContext<DeviceAuthFailed> context)
    {
        logger.LogDebug("Received DeviceAuthFailed event for {Id}", context.Message.DeviceId);
        metricsService.IncrementAuthFailed(deviceId: context.Message.DeviceId, reason: context.Message.Reason);
        return Task.CompletedTask;
    }
}