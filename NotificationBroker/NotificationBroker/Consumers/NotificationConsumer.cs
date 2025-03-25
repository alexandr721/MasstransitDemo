using Demo.Shared.Contracts;
using MassTransit;

namespace NotificationBroker.Consumers
{
    public class NotificationConsumer(ILogger<NotificationConsumer> logger) : IConsumer<NotificationMessage>
    {
        private ConsumeContext<NotificationMessage>? _contextMq;
        public async Task Consume(ConsumeContext<NotificationMessage> contextMq)
        {
            _contextMq = contextMq;
            if (contextMq == null)
            {
                logger.LogWarning("Received empty context");
                return;
            }
            var message = contextMq.Message;
            logger.LogInformation($"Received notification: {message.UserId} ({message.Type})");

            try
            {
                switch (message.Type)
                {
                    case "email":
                        await SendEmailNotificationAsync(message, contextMq);
                        break;
                    case "sms":
                        await SendSmsNotificationAsync(message, contextMq);
                        break;
                    case "telegram":
                        await SendTelegramNotificationAsync(message, contextMq);
                        break;
                    default:
                        logger.LogWarning($"Unknown notification type: {message.Type}");
                        break;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to process notification");
            }
        }

        private async Task SendEmailNotificationAsync(
            NotificationMessage message, 
            ConsumeContext<NotificationMessage> contextMq)
        {
            var msg =new EmailNotificationMessage
            {
                UserId = message.UserId,
                Username = message.Username,
                Email = message.Email,
                Message = message.Message
            };
            // Publish to the exchange configured for this message type
            await contextMq.Publish(msg);
            logger.LogInformation($"Published {message.Type} message for {message.UserId}");
        }

        private async Task SendSmsNotificationAsync(
            NotificationMessage message,
            ConsumeContext<NotificationMessage> contextMq)
        {
            var msg =new SMSNotificationMessage
            {
                UserId = message.UserId,
                Username = message.Username,
                Email = message.Email,
                Message = message.Message
            };
            await contextMq.Publish(msg);
            logger.LogInformation($"Published {message.Type} message for {message.UserId}");
        }

        private async Task SendTelegramNotificationAsync(
            NotificationMessage message,
            ConsumeContext<NotificationMessage> contextMq)
        {
            var msg =new TelegramNotificationMessage
            {
                UserId = message.UserId,
                Username = message.Username,
                TelegrammId = message.TelegrammId,
                Message = message.Message
            };
            await contextMq.Publish(msg);
            logger.LogInformation($"Published {message.Type} message for {message.UserId}");
        }
    }
}
