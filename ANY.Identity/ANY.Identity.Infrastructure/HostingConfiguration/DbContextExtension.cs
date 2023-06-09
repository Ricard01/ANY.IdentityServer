using ANY.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class DbContextExtension
{
    public static void ConfigDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var mySqlConnectionStr = configuration.GetConnectionString("MySQL");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
        
    }
}