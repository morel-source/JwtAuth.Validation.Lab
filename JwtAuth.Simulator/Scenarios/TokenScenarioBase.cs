namespace JwtAuth.Simulator.Scenarios;

public abstract class TokenScenarioBase : ITokenScenario
{
    protected const int CountScenarios = 30;

    public List<DeviceDetails> GetTokenScenario()
    {
        return GetScenarios();
    }

    protected abstract List<DeviceDetails> GetScenarios();
}