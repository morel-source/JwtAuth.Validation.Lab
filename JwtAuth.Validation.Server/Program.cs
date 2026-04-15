using JwtAuth.Shared;
using JwtAuth.Shared.Validator;
using JwtAuth.Validation.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IJwtValidator, JwtValidator>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddHostedService<Server>();

var app = builder.Build();
app.Run();