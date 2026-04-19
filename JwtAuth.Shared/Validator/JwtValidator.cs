using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Shared.Validator;

public sealed class JwtValidator(
    ILogger<JwtValidator> logger,
    IOptions<JwtOptions> options
) : IJwtValidator
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();
    private readonly byte[] _keyBytes = Encoding.ASCII.GetBytes(options.Value.Key);
    private const int MaxTokenLength = 2048;

    public ValidationResult TryValidateToken(string token)
    {
        string deviceId = "Unknown";

        if (string.IsNullOrWhiteSpace(token) || token.Length > MaxTokenLength)
        {
            logger.LogWarning("[FAILED] Token invalid length. Request rejected.");
            return new ValidationResult(deviceId, IsSuccess: false, "Token invalid length");
        }

        if (TokenHandler.CanReadToken(token))
        {
            var jwtToken = TokenHandler.ReadJwtToken(token);
            var deviceClaim =
                jwtToken.Claims.FirstOrDefault(c => c.Type is "nameid" or ClaimTypes.NameIdentifier or "sub");
            deviceId = deviceClaim?.Value ?? "Unknown";
        }

        try
        {
            TokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            logger.LogInformation("[SUCCESS] Token Validated for Device: {DeviceID}", deviceId);
            return new ValidationResult(deviceId, IsSuccess: true, Reason: "Success");
        }
        catch (SecurityTokenExpiredException)
        {
            logger.LogInformation("[FAILED] Token expired. Request rejected.");
            return new ValidationResult(deviceId, IsSuccess: false,
                Reason: "Lifetime validation failed. The token is expired.");
        }
        catch (SecurityTokenSignatureKeyNotFoundException)
        {
            logger.LogInformation("[FAILED] Token Invalid. Request rejected.");
            return new ValidationResult(deviceId, IsSuccess: false,
                Reason: "Signature validation failed. The token's kid is missing.");
        }
        catch (Exception ex)
        {
            logger.LogWarning("[FAILED] Validation failed for Device: {DeviceID}. Reason: {Reason}", deviceId,
                ex.Message);
            return new ValidationResult(deviceId, IsSuccess: false, Reason: ex.Message);
        }
    }
}