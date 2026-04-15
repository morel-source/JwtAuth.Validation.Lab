namespace JwtAuth.Shared.Validator;

public interface IJwtValidator
{
    void TryValidateToken(string token, out string deviceBarcode);
}