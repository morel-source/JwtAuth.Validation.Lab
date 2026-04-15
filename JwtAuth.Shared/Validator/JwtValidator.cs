using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Shared.Validator;

public class JwtValidator(
    ILogger<JwtValidator> logger,
    IOptions<JwtOptions> options
) : IJwtValidator
{
    private readonly byte[] _keyBytes = Encoding.ASCII.GetBytes(options.Value.Key);

    public void TryValidateToken(string token, out string deviceBarcode)
    {
        deviceBarcode = string.Empty;
        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);

            deviceBarcode = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";

            logger.LogInformation("[SUCCESS] Token Validated for Device: {DeviceID}", deviceBarcode);
        }
        catch (Exception ex)
        {
            logger.LogWarning("[FAILED] Token Validation failed. Reason: {Reason}", ex.Message);
        }
    }
}