namespace JwtAuth.Shared.Manager;

public interface IJwtTokenManager
{
    string? Authenticate(string deviceBarcode, string? overrideKey = null);
}