using JwtAuth.Shared;
using JwtAuth.Shared.Manager;
using JwtAuth.Simulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddHostedService<MultiDeviceSimulator>();

var app = builder.Build();
app.Run();