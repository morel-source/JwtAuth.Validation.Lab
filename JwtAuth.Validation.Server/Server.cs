using System.Net.Sockets;
using System.Text;
using JwtAuth.Shared;
using JwtAuth.Shared.Validator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtAuth.Validation.Server;

public sealed class Server(
    ILogger<Server> logger,
    IOptions<TcpOptions> tcpOptions,
    IJwtValidator jwtValidator,
    ConnectionManager connectionManager
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var listener = TcpListener.Create(tcpOptions.Value.ListeningPort);
        listener.Start();
        logger.LogInformation("Validator Server listening on port {Port}...", tcpOptions.Value.ListeningPort);

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(stoppingToken).ConfigureAwait(false);
            _ = Task.Run(() => HandleClientConnection(client, stoppingToken), stoppingToken);
        }
    }

    private async Task HandleClientConnection(TcpClient client, CancellationToken stoppingToken)
    {
        string? deviceId = null;
        try
        {
            var stream = client.GetStream();
            var buffer = new byte[2048];

            var bytesRead = await stream.ReadAsync(buffer, stoppingToken).ConfigureAwait(false);
            if (bytesRead == 0) return;

            var token = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var result = jwtValidator.TryValidateToken(token);

            if (result.IsSuccess)
            {
                deviceId = result.DeviceId;
                await connectionManager.RegisterConnection(deviceId, client, stoppingToken).ConfigureAwait(false);

                logger.LogInformation("Device {Id} authenticated and session started.", deviceId);

                while (await stream.ReadAsync(buffer, stoppingToken).ConfigureAwait(false) > 0)
                {
                    // stay the connection open
                }
            }
            else
            {
                await connectionManager.ConnectionAuthFailed(result.DeviceId, result.Reason, stoppingToken)
                    .ConfigureAwait(false);
                client.Dispose();
            }
        }
        catch (Exception ex)
        {
            logger.LogDebug("Session ended for {Id}: {Msg}", deviceId ?? "unknown", ex.Message);
        }
        finally
        {
            if (deviceId != null)
            {
                await connectionManager.UnregisterConnection(deviceId, stoppingToken).ConfigureAwait(false);
            }

            client.Dispose();
        }
    }
}