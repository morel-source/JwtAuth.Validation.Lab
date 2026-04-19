namespace Analytics.Worker.Monitoring;

public interface IMetricsService
{
    void SetActiveConnectionCount(int count);
    void IncrementAuthSuccess();
    void IncrementAuthFailed(string reason, string deviceId);
}