using Demo.Shared.Contracts;
using MassTransit;
using OrderService.Data;
using OrderService.Models;

namespace OrderService.Consumers;

public class UserRegisteredConsumer(
    OrderDbContext context,
    ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> contextMq)
    {
        var message = contextMq.Message;
        logger.LogInformation($"Received user's registration message: {message.UserId}");

        var user = new User
        {
            UserId = message.UserId,
            Email = message.Email,
            Role = message.Role,
            PhoneNumber = message.PhoneNumber,
            Username = message.Username
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        logger.LogInformation($"User {message.Username} is stored into DB!");
    }
}
