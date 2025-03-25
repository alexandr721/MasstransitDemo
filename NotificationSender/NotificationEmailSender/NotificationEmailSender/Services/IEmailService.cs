using Demo.Shared.Contracts;

namespace NotificationEmailSender.Services
{
    public interface IEmailService
    {
        Task SendRegistrationEmailAsync(EmailNotificationMessage emailNotification);
    }
}

