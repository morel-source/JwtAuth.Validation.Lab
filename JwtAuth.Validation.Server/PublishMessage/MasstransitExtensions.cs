using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Events;

namespace JwtAuth.Validation.Server.PublishMessage;

public static class MasstransitExtensions
{
    public static void AddMasstransit(this HostApplicationBuilder builder)
    {
        builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));

        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                cfg.Host(options.Host, "/", h =>
                {
                    h.Username(options.UserName);
                    h.Password(options.Password);
                });

                cfg.UseMessageRetry(r =>
                    r.Exponential(
                        retryLimit: 5,
                        minInterval: TimeSpan.FromSeconds(1),
                        maxInterval: TimeSpan.FromSeconds(30),
                        intervalDelta: TimeSpan.FromSeconds(2))
                );

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}