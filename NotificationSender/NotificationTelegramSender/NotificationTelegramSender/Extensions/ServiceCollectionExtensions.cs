using MassTransit;
using Microsoft.Extensions.Options;
using NotificationSmsSender.Consumers;
using NotificationSmsSender.Models;

namespace NotificationSmsSender.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<NotificationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = context.GetRequiredService<IOptions<RabbitConfig>>().Value;
                cfg.Host(rabbitConfig.Host, h =>
                {
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });

                // Привязываем очередь к фан-аут Exchange
                cfg.ReceiveEndpoint(rabbitConfig.QueueName, e =>
                {
                    e.Bind(rabbitConfig.ExchangeName, x => x.ExchangeType = "fanout");
                    e.ConfigureConsumer<NotificationConsumer>(context);
                });
            });
        });
        return services;
    }
}