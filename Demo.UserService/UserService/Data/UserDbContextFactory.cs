using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.Data;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        optionsBuilder.UseNpgsql(connectionString);

        return new UserDbContext(optionsBuilder.Options);
    }
}
    
