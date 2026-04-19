namespace Shared.Events;

public sealed record DeviceAuthFailed(string DeviceId, string Reason, DateTime Timestamp);