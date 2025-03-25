using Demo.Shared.Contracts;
using MassTransit;

namespace NotificationEmailSender.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<EmailNotificationMessage>
    {
        public async Task Consume(ConsumeContext<EmailNotificationMessage> contextMq)
        {
            var message = contextMq.Message;
            await Task.Delay(1000);
            //logger.LogInformation($"Received notification message: {message.UserId}");
            logger.LogInformation($"Sent email notification: {message.UserId}");
        }
    }
}