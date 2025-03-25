using NotificationSmsSender.Extensions;
using NotificationSmsSender.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.Configure<RabbitConfig>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddRabbit();

var app = builder.Build();
app.Run();