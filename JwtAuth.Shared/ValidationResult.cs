namespace JwtAuth.Shared;

public readonly record struct ValidationResult(string DeviceId, bool IsSuccess, string Reason);