using Demo.Shared.Contracts;
using MassTransit;

namespace NotificationSmsSender.Consumers;

public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<TelegramNotificationMessage>
{
    public async Task Consume(ConsumeContext<TelegramNotificationMessage> contextMq)
    {
        var message = contextMq.Message;
        await Task.Delay(1000);
        //logger.LogInformation($"Received notification message: {message.UserId}");
        logger.LogInformation($"Sent Telegram notification: {message.UserId}");
    }
}