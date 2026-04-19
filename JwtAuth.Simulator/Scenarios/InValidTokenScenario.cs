using JwtAuth.Shared.Manager;

namespace JwtAuth.Simulator.Scenarios;

public sealed class InValidTokenScenario(IJwtTokenManager tokenManager) : TokenScenarioBase
{
    private const string InvalidToken = "ThisIsMyWRONG_KEY_123456789012345678901";
    protected override List<DeviceDetails> GetScenarios()
    {
        var list = new List<DeviceDetails>(capacity: CountScenarios);
        for (int i = 1; i <= CountScenarios; i++)
        {
            var deviceId = $"InValid_Device_{i}";
            var token = tokenManager.Authenticate(deviceId, overrideKey:InvalidToken);
            list.Add(new DeviceDetails(deviceId, token));
        }

        return list;
    }
}