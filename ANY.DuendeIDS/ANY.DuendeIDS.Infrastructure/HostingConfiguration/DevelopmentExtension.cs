using ANY.DuendeIDS.Infrastructure.Persistence;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ANY.DuendeIDS.Infrastructure.HostingConfiguration;

public static class DevelopmentExtension
{
    public static void ConfigDevelopExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var mySqlConnectionStr = configuration.GetConnectionString("MySQL");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(mySqlConnectionStr ,ServerVersion.AutoDetect(mySqlConnectionStr))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });


        services.AddTransient<IRedirectUriValidator, RedirectValidator>();
    }
}

internal class RedirectValidator : IRedirectUriValidator
{
    public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(true);
    }
}