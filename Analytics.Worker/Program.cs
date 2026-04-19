using Analytics.Worker;
using Analytics.Worker.Monitoring.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.AddMasstransit();
builder.UseMonitoring();
var app = builder.Build();
app.Run();