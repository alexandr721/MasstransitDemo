using Demo.Shared.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

namespace OrderService.Consumers
{
    public class UserLoggedOutConsumer(
        OrderDbContext context,
        ILogger<UserLoggedOutConsumer> logger) : IConsumer<UserLoggedOutEvent>
    {
        public async Task Consume(ConsumeContext<UserLoggedOutEvent> contextMq)
        {
            var message = contextMq.Message;
            logger.LogInformation($"Received user's logout message: {message.UserId}");

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == message.UserId);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                logger.LogInformation($"User {user.Username} has been removed from DB.");
            }
            else
            {
                logger.LogWarning($"User ID {message.UserId} is not found.");
            }
        }
    }
}