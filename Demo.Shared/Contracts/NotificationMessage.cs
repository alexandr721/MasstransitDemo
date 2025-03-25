namespace Demo.Shared.Contracts
{
    public class NotificationMessage
    {
        public string Type { get; set; } = string.Empty; // email, sms, telegram, event, log, billing
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? TelegrammId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty; // Текст уведомления
    }
}

