using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Extensions;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Подключаем PostgreSQL через EF Core
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RabbitConfig>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddRabbit();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Настраиваем API и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate(); // Применит все миграции
}
app.Run();