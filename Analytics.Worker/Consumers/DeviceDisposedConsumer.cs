using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Analytics.Worker.Consumers;

public sealed class DeviceDisposedConsumer(
    ILogger<DeviceDisposedConsumer> logger
) : IConsumer<DeviceDisposed>
{
    public Task Consume(ConsumeContext<DeviceDisposed> context)
    {
        logger.LogDebug("Device {Id} disconnected", context.Message.DeviceId);
        return Task.CompletedTask;
    }
}