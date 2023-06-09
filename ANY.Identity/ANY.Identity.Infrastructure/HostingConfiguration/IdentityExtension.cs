using ANY.DuendeIDS.Domain.Entities;
using ANY.Identity.Infrastructure.Common.Interfaces;
using ANY.Identity.Infrastructure.Persistence;
using ANY.Identity.Infrastructure.Repositories.Users;
using ANY.Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class IdentityExtension
{
    public static void ConfigIdentity(this IServiceCollection services)
    {
        // UserManager RoleManger for CRUD 
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();


        services.AddSingleton<ICurrentUserService, CurrentUserService>();
    }
}