using System.Collections.Concurrent;
using System.Net.Sockets;
using JwtAuth.Validation.Server.PublishMessage;
using Microsoft.Extensions.Logging;

namespace JwtAuth.Validation.Server;

public sealed class ConnectionManager(
    ILogger<ConnectionManager> logger,
    IPublishMessage publishMessage)
{
    private readonly ConcurrentDictionary<string, TcpClient> _activeConnections = new();

    public async Task RegisterConnection(string deviceId, TcpClient client,
        CancellationToken cancellationToken = default)
    {
        _activeConnections.AddOrUpdate(deviceId, client, (_, oldClient) =>
        {
            oldClient.Dispose();
            return client;
        });

        await publishMessage.PublishDeviceAuthenticated(deviceId, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Device {Id} registered.", deviceId);
        SyncMetrics(cancellationToken);
    }

    public async Task UnregisterConnection(string deviceId, CancellationToken cancellationToken = default)
    {
        if (_activeConnections.TryRemove(deviceId, out var client))
        {
            try
            {
                client.Dispose();
            }
            catch
            {
                /* Ignore */
            }

            logger.LogInformation("Device {Id} unregistered and disposed.", deviceId);
            await publishMessage.PublishDeviceDisposed(deviceId, cancellationToken).ConfigureAwait(false);
            SyncMetrics(cancellationToken);
        }
    }

    public async Task ConnectionAuthFailed(string deviceId, string reason,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Device {Id} auth Failed.", deviceId);
        await publishMessage.PublishDeviceAuthFailed(deviceId, reason, cancellationToken).ConfigureAwait(false);
    }

    private void SyncMetrics(CancellationToken cancellationToken = default)
    {
        publishMessage.PublishActiveConnectionCount(_activeConnections.Count, cancellationToken).ConfigureAwait(false);
    }
}