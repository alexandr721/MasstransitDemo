using AuthService.Data;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using Demo.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IPublishEndpoint publishEndpoint,
    AuthDbContext context,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisteredEvent user)
    {
        user.UserId = Guid.NewGuid(); // Генерируем новый ID
        await context.Users.AddAsync(new User()
        {
            UserId = user.UserId,
            Email = user.Email,
            Username = user.Username,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        });
        await context.SaveChangesAsync();
        logger.LogInformation($"User {user.UserId} {user.Username} registered successfully.");

        await publishEndpoint.Publish(user);
        var message = new NotificationMessage()
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = DateTime.Now,
            IsActive = true,
            RoleId = 1,
            RoleName = "User",
            Message = "User registered successfully",
            Type = "email"
        };
        await publishEndpoint.Publish(message);
        logger.LogInformation($"Email {user.UserId} {user.Username} published successfully.");
        message.Type = "sms";
        await publishEndpoint.Publish(message);
        logger.LogInformation($"SMS {user.UserId} {user.Username} published successfully.");
        message.Type = "telegram";
        await publishEndpoint.Publish(message);
        logger.LogInformation($"Telegram {user.UserId} {user.Username} published successfully.");
        return Ok($"User {user.UserId} {user.Username} registered successfully.");
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(Guid userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            return NotFound();
        }
        await context.Users.Where(u => u.UserId == userId).ExecuteDeleteAsync();
        await context.SaveChangesAsync();
        logger.LogInformation($"User {user.UserId} {user.Username} logged out successfully.");
        await publishEndpoint.Publish(new UserLoggedOutEvent
        {
            UserId = userId
        });
        logger.LogInformation($"User {user.UserId} {user.Username} published successfully.");
        return Ok($"User {user.UserId} {user.Username} logged out successfully.");
    }
}