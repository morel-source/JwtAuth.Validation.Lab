using Analytics.Worker.Consumers;
using MassTransit;

namespace Analytics.Worker.Definitions;

public sealed class DeviceAuthenticatedConsumerDefinition : ConsumerDefinition<DeviceAuthenticatedConsumer>
{
    public DeviceAuthenticatedConsumerDefinition()
    {
        EndpointName = "device-authenticated-queue";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeviceAuthenticatedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    }
}