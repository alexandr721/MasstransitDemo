using Demo.Shared.Contracts;
using MassTransit;

namespace NotificationSmsSender.Consumers;

public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<SMSNotificationMessage>
{
    public async Task Consume(ConsumeContext<SMSNotificationMessage> contextMq)
    {
        var message = contextMq.Message;
        await Task.Delay(1000);
        //logger.LogInformation($"Received notification message: {message.UserId}");
        logger.LogInformation($"Sent sms notification: {message.UserId}");
    }
}