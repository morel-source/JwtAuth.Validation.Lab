using JwtAuth.Shared.Manager;

namespace JwtAuth.Simulator.Scenarios;

public sealed class ExpireTokenScenario(IJwtTokenManager tokenManager) : TokenScenarioBase
{
    protected override List<DeviceDetails> GetScenarios()
    {
        var list = new List<DeviceDetails>(capacity: CountScenarios);
        var expireDateTime = DateTime.UtcNow.AddMinutes(-10);
        var notBeforeDateTime = DateTime.UtcNow.AddMinutes(-30);
        for (int i = 1; i <= CountScenarios; i++)
        {
            var deviceId = $"Expire_Device_{i}";
            var token = tokenManager.Authenticate(deviceId: deviceId, expires: expireDateTime, notBefore: notBeforeDateTime);
            list.Add(new DeviceDetails(deviceId, token));
        }

        return list;
    }
}