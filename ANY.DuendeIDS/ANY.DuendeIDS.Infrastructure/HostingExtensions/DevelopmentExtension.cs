using ANY.DuendeIDS.Infrastructure.Persistence;
using ANY.DuendeIDS.Infrastructure.Services;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.HostingExtensions;

public static class DevelopmentExtension
{
    public static void DevelopmentConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var mySqlConnectionStr = configuration.GetConnectionString("MySQL");
       
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });

        // services.AddTransient<IAuthorizationService, CheckAuthorizationService>();
        services.AddTransient<IRedirectUriValidator, RedirectValidator>();

        // services.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();
    }
}

// Checks if something when work with redirection on Identity Clients 
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