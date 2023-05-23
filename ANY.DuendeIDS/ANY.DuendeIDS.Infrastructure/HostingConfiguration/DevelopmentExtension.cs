using ANY.DuendeIDS.Infrastructure.Persistence;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ANY.DuendeIDS.Infrastructure.HostingConfiguration;

public static class DevelopmentExtension
{
    public static void ConfigDevelopExtension(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase("AnyDb"); });
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