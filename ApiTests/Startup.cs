using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ApiTests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddAuthentication("token")
            .AddJwtBearer("token", options =>
            {
                options.Authority = "https://localhost:5000";
                options.Audience = "identityApi";
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateActor = true,

                    ValidateIssuerSigningKey = true,
                    ValidTypes = new[] { "at+jwt" },
                    // NameClaimType = "name",
                    // RoleClaimType = "role"
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiCaller", policy => { policy.RequireClaim("scope", "IdentityServerApi"); });

            options.AddPolicy("RequireInteractiveUser", policy => { policy.RequireClaim("sub"); });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // sigue funcionado aun con esto comentado
        // app.UseForwardedHeaders(new ForwardedHeadersOptions
        // {
        //     ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
        // });

        app.UseSerilogRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            IdentityModelEventSource.ShowPII = true;
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers()
                .RequireAuthorization("ApiCaller");
        });
    }
}