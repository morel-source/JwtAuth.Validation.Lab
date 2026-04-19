using Analytics.Worker.Consumers;
using MassTransit;

namespace Analytics.Worker.Definitions;

public sealed class DeviceDisposedConsumerDefinition : ConsumerDefinition<DeviceDisposedConsumer>
{
    public DeviceDisposedConsumerDefinition()
    {
        EndpointName = "device-disposed-queue";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<DeviceDisposedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    }
}