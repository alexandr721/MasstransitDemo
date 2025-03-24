using AuthService.Models;
using Demo.Shared.Contracts;
using MassTransit;
using Microsoft.Extensions.Options;

namespace AuthService.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRabbit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = context.GetRequiredService<IOptions<RabbitConfig>>().Value;
                cfg.Host(rabbitConfig.Host, h =>
                {
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });

                // Publish in Exchange (Fanout)
                cfg.Message<UserRegisteredEvent>(e => e.SetEntityName(rabbitConfig.ExchangeName));
                cfg.Message<UserLoggedOutEvent>(e => e.SetEntityName(rabbitConfig.ExchangeName));
            });
        });
        return services;
    }    
}