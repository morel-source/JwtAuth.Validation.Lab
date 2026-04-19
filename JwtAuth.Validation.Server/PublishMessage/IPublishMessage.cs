namespace JwtAuth.Validation.Server.PublishMessage;

public interface IPublishMessage
{
    Task PublishDeviceAuthenticated(string deviceId, CancellationToken cancellationToken = default);
    Task PublishDeviceAuthFailed(string deviceId, string reason, CancellationToken cancellationToken = default);
    Task PublishDeviceDisposed(string deviceId, CancellationToken cancellationToken = default);
    Task PublishActiveConnectionCount(int count, CancellationToken cancellationToken = default);
}