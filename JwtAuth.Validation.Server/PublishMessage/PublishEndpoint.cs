using MassTransit;
using Shared.Events;

namespace JwtAuth.Validation.Server.PublishMessage;

public sealed class PublishMessage(IPublishEndpoint publishEndpoint) : IPublishMessage
{
    public async Task PublishDeviceAuthenticated(string deviceId, CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(new DeviceAuthenticated(deviceId, DateTime.UtcNow), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task PublishDeviceAuthFailed(string deviceId, string reason,
        CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(new DeviceAuthFailed(deviceId, reason, DateTime.UtcNow), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task PublishDeviceDisposed(string deviceId, CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(new DeviceDisposed(deviceId), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task PublishActiveConnectionCount(int count, CancellationToken cancellationToken = default)
    {
        await publishEndpoint.Publish(new ConnectionCountUpdated(count), cancellationToken)
            .ConfigureAwait(false);
    }
}