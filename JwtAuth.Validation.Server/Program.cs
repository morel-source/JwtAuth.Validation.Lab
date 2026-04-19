using JwtAuth.Shared;
using JwtAuth.Shared.Validator;
using JwtAuth.Validation.Server;
using JwtAuth.Validation.Server.PublishMessage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddSingleton<IJwtValidator, JwtValidator>();
builder.Services.AddSingleton<IPublishMessage, PublishMessage>();
builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddHostedService<Server>();
builder.AddMasstransit();

var app = builder.Build();
app.Run();