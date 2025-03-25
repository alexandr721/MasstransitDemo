using Demo.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Options;
using NotificationBroker.Consumers;
using NotificationBroker.Models;

namespace NotificationBroker.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            // Register the consumer that handles NotificationMessage only
            x.AddConsumer<NotificationConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = context.GetRequiredService<IOptions<RabbitConfig>>().Value;

                cfg.Host(rabbitConfig.Host, h =>
                {
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });

                // This service consumes only NotificationMessage
                cfg.ReceiveEndpoint(rabbitConfig.QueueName, e =>
                {
                    e.Bind(rabbitConfig.ExchangeName, x => x.ExchangeType = "fanout");
                    e.ConfigureConsumer<NotificationConsumer>(context);
                });

                // The following messages are published, but NOT consumed here
                // We set target exchanges for each type
                cfg.Message<TelegramNotificationMessage>(x => x.SetEntityName("telegram-exchange"));
                cfg.Message<SMSNotificationMessage>(x => x.SetEntityName("sms-exchange"));
                cfg.Message<EmailNotificationMessage>(x => x.SetEntityName("email-exchange"));
            });
        });

        return services;
    }
}