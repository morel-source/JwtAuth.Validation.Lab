using Analytics.Worker.Consumers;
using MassTransit;

namespace Analytics.Worker.Definitions;

public sealed class DeviceAuthFailedConsumerDefinition : ConsumerDefinition<DeviceAuthFailedConsumer>
{
    public DeviceAuthFailedConsumerDefinition()
    {
        EndpointName = "device-auth-failed-queue";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeviceAuthFailedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    }
}