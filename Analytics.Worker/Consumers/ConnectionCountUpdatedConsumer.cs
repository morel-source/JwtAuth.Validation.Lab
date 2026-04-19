using Analytics.Worker.Monitoring;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Analytics.Worker.Consumers;

public sealed class ConnectionCountUpdatedConsumer(
    ILogger<ConnectionCountUpdatedConsumer> logger,
    IMetricsService metricsService
) : IConsumer<ConnectionCountUpdated>
{
    public Task Consume(ConsumeContext<ConnectionCountUpdated> context)
    {
        logger.LogDebug("Received ConnectionCountUpdated event for count {Count}", context.Message.Count);
        metricsService.SetActiveConnectionCount(context.Message.Count);
        return Task.CompletedTask;
    }
}