using ANY.DuendeIDS.Infrastructure.Common.Interfaces;
using ANY.DuendeIDS.Infrastructure.Repositories.Users;
using ANY.DuendeIDS.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.DuendeIDS.Infrastructure.HostingExtensions;

public static class RepositoriesExtension
{
    public static void AddUserRoleServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
    }
}