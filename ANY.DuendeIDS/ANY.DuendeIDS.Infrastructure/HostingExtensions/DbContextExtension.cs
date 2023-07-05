using ANY.DuendeIDS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace  ANY.DuendeIDS.Infrastructure.HostingExtensions;

public static class DbContextExtension
{
    public static void DbContextConfiguration(this IServiceCollection services)
    {
       
        services.AddDbContext<ApplicationDbContext>(options =>
        {

            options.UseInMemoryDatabase("AnyDb");

        });

    }
   
}

