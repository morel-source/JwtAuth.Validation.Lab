using Prometheus;

namespace Analytics.Worker.Monitoring;

public static class MetricsRegistry
{
    public static readonly Gauge ActiveConnections =
        Metrics.CreateGauge(
            name: "active_connections_total",
            help: "Number of currently active TCP connections"
        );

    public static readonly Counter AuthAttempts =
        Metrics.CreateCounter(
            name: "auth_attempts_total",
            help: "Total auth attempts"
        );

    public static readonly Counter AuthSuccess =
        Metrics.CreateCounter(
            name: "auth_success_total",
            help: "Total successful authentications"
        );

    public static readonly Counter AuthFailures =
        Metrics.CreateCounter(
            name: "auth_failures_total",
            help: "Total failed authentications",
            new CounterConfiguration { LabelNames = ["reason", "device_id"] }
        );
}