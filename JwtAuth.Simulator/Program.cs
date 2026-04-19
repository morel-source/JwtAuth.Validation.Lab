using JwtAuth.Shared;
using JwtAuth.Shared.Manager;
using JwtAuth.Simulator;
using JwtAuth.Simulator.Scenarios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddSingleton<ITokenScenario, ValidTokenScenario>();
builder.Services.AddSingleton<ITokenScenario, InValidTokenScenario>();
builder.Services.AddSingleton<ITokenScenario, ExpireTokenScenario>();
builder.Services.AddHostedService<MultiDeviceSimulator>();

var app = builder.Build();
app.Run();