namespace Demo.Shared.Contracts;

public class TelegramNotificationMessage
{
    public string Type { get; } = "telegram"; // email, sms, telegram, event, log, billing
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? TelegrammId { get; set; }
    public string Message { get; set; } = string.Empty; // Текст уведомления
}