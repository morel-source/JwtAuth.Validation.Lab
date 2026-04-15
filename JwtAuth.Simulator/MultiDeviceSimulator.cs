using System.Net.Sockets;
using System.Text;
using JwtAuth.Shared;
using JwtAuth.Shared.Manager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace JwtAuth.Simulator;

public class MultiDeviceSimulator(
    ILogger<MultiDeviceSimulator> logger,
    IOptions<TcpOptions> tcpOptions,
    IJwtTokenManager tokenManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var devices = new List<Task>();

        // create valid token devices
        for (int i = 1; i <= 10; i++)
            devices.Add(SimulateDevice(i, isValid: true, stoppingToken));

        // create invalid token devices
        for (int i = 1; i <= 10; i++)
            devices.Add(SimulateDevice(i, isValid: false, stoppingToken));

        await Task.WhenAll(devices);
    }

    private async Task SimulateDevice(int id, bool isValid, CancellationToken cancellationToken)
    {
        var deviceId = $"{(isValid ? "Valid" : "Invalid")}_Device_{id}";
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var token = isValid
                    ? tokenManager.Authenticate(deviceId)
                    : tokenManager.Authenticate(deviceBarcode: deviceId, overrideKey: "ThisIsMyWRONG_KEY_123456789012345678901");

                using var client = new TcpClient(tcpOptions.Value.Host, tcpOptions.Value.ListeningPort);
                await using var stream = client.GetStream();
                var data = Encoding.UTF8.GetBytes(token!);
                await stream.WriteAsync(data, cancellationToken);

                logger.LogInformation("{DeviceId} sent token.", deviceId);
            }
            catch (Exception ex)
            {
                logger.LogError("Device {DeviceId} failed to connect: {Msg}", deviceId, ex.Message);
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }
}