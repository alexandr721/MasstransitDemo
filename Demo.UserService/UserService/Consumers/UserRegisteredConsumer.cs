using Demo.Shared.Contracts;
using MassTransit;
using UserService.Data;
using UserService.Models;

namespace UserService.Consumers;

public class UserRegisteredConsumer(
    UserDbContext context,
    ILogger<UserRegisteredConsumer> logger) : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> contextMq)
    {
        var message = contextMq.Message;
        logger.LogInformation($"Received user's registration message: {message.UserId}");

        var user = new User
        {
            Id = message.UserId,
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
