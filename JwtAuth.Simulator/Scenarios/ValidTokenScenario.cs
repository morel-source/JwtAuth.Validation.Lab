using JwtAuth.Shared.Manager;

namespace JwtAuth.Simulator.Scenarios;

public sealed class ValidTokenScenario(IJwtTokenManager tokenManager) : TokenScenarioBase
{
    protected override List<DeviceDetails> GetScenarios()
    {
        var list = new List<DeviceDetails>(capacity: CountScenarios);
        for (int i = 1; i <= CountScenarios; i++)
        {
            var deviceId = $"Valid_Device_{i}";
            var token = tokenManager.Authenticate(deviceId);
            list.Add(new DeviceDetails(deviceId, token));
        }

        return list;
    }
}