using Analytics.Worker.Monitoring.Configuration;

namespace Analytics.Worker.Monitoring;

public sealed class PrometheusMetricsService : IMetricsService
{
    public void SetActiveConnectionCount(int count)
    {
        MetricsRegistry.ActiveConnections.Set(count);
    }

    public void IncrementAuthSuccess()
    {
        MetricsRegistry.AuthAttempts.Inc();
        MetricsRegistry.AuthSuccess.Inc();
    }

    public void IncrementAuthFailed(string reason, string deviceId)
    {
        MetricsRegistry.AuthAttempts.Inc();
        MetricsRegistry.AuthFailures.WithLabels(reason, deviceId).Inc();
    }
}