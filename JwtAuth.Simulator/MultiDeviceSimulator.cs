using System.Net.Sockets;
using System.Text;
using JwtAuth.Shared;
using JwtAuth.Simulator.Scenarios;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtAuth.Simulator;

public sealed class MultiDeviceSimulator(
    ILogger<MultiDeviceSimulator> logger,
    IOptions<TcpOptions> tcpOptions,
    IEnumerable<ITokenScenario> tokenScenarios
) : BackgroundService
{
    private readonly SemaphoreSlim _connectionSemaphore = new(100);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var devices = new List<Task>();

        foreach (var scenario in tokenScenarios)
        {
            foreach (var device in scenario.GetTokenScenario())
            {
                devices.Add(SimulateDevice(device, stoppingToken));
            }
        }

        await Task.WhenAll(devices).ConfigureAwait(false);
    }


    private async Task SimulateDevice(DeviceDetails deviceDetails, CancellationToken cancellationToken)
    {
        var random = new Random();

        while (!cancellationToken.IsCancellationRequested)
        {
            await _connectionSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(tcpOptions.Value.Host, tcpOptions.Value.ListeningPort, cancellationToken)
                    .ConfigureAwait(false);

                if (client.Connected)
                {
                    await using var stream = client.GetStream();
                    var data = Encoding.UTF8.GetBytes(deviceDetails.Token);
                    await stream.WriteAsync(data, cancellationToken).ConfigureAwait(false);

                    logger.LogInformation("{DeviceId} connected and sent token.", deviceDetails.DeviceId);

                    // stay the connection open for 30 - 60 sec
                    var stayConnectedTime = TimeSpan.FromSeconds(random.Next(30, 60));
                    await Task.Delay(stayConnectedTime, cancellationToken).ConfigureAwait(false);

                    logger.LogInformation("{DeviceId} session finished, closing connection.", deviceDetails.DeviceId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Device {DeviceId} error: {Msg}", deviceDetails.DeviceId, ex.Message);
            }
            finally
            {
                _connectionSemaphore.Release();
            }

            // time between the connection to new connection
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken).ConfigureAwait(false);
        }
    }
}