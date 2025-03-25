namespace Demo.Shared.Contracts;

public class SMSNotificationMessage
{
    public string Type { get; } = "sms"; // email, sms, telegram, event, log, billing
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Message { get; set; } = string.Empty; // Текст уведомления
}