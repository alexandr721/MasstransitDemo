using System;

namespace AuthService.Models;

public class User
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
}