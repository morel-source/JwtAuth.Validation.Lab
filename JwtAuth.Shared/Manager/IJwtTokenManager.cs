namespace JwtAuth.Shared.Manager;

public interface IJwtTokenManager
{
    string Authenticate(string deviceId, string? overrideKey = null, DateTime? expires = null,
        DateTime? notBefore = null);
}