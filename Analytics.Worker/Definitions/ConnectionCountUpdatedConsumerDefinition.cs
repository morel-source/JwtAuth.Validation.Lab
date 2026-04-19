using Analytics.Worker.Consumers;
using MassTransit;

namespace Analytics.Worker.Definitions;

public sealed class ConnectionCountUpdatedConsumerDefinition : ConsumerDefinition<ConnectionCountUpdatedConsumer>
{
    public ConnectionCountUpdatedConsumerDefinition()
    {
        EndpointName = "connection-count-updated-queue";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<ConnectionCountUpdatedConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
    }
}