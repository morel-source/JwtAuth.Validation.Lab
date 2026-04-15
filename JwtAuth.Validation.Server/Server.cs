using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using JwtAuth.Shared;
using JwtAuth.Shared.Validator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtAuth.Validation.Server;

public class Server(
    ILogger<Server> logger,
    IOptions<TcpOptions> tcpOptions,
    IJwtValidator jwtValidator) : BackgroundService
{
    private readonly Channel<string> _channel = Channel.CreateUnbounded<string>();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = TcpListener.Create(tcpOptions.Value.ListeningPort);
        listener.Start();
        logger.LogInformation("Validator Server listening on port {Port}...", tcpOptions.Value.ListeningPort);

        _ = Task.Run(async () => await ValidateToken(stoppingToken), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(stoppingToken);

            _ = Task.Run(async () =>
            {
                using (client)
                {
                    var stream = client.GetStream();
                    var buffer = new byte[2048];
                    var bytesRead = await stream.ReadAsync(buffer, stoppingToken);

                    if (bytesRead > 0)
                    {
                        var token = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        await _channel.Writer.WriteAsync(token, stoppingToken);
                    }
                }
            }, stoppingToken);
        }
    }

    private async Task ValidateToken(CancellationToken cancellationToken = default)
    {
        await foreach (var token in _channel.Reader.ReadAllAsync(cancellationToken))
        {
            jwtValidator.TryValidateToken(token, out var barcode);

            if (!string.IsNullOrEmpty(barcode) && barcode != "Unknown")
            {
                logger.LogDebug("Background processing completed for Device: {Barcode}", barcode);
            }
        }
    }
}