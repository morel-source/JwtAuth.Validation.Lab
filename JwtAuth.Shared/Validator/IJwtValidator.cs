namespace JwtAuth.Shared.Validator;

public interface IJwtValidator
{
    ValidationResult TryValidateToken(string token);
}