using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ANY.Identity.Infrastructure.HostingConfiguration;

public static class AuthenticationExtension
{
    public static void ConfigAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:7001";
                options.Audience = "identityApi";

                // it's recommended to check the type header to avoid "JWT confusion" attacks
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidTypes = new[] { "at+jwt" }
                };

                options.MapInboundClaims = false;
                // options.Events = new JwtBearerEvents()
                // {
                //     OnChallenge = context =>
                //     {
                //         context.HandleResponse();
                //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //         context.Response.ContentType = "application/json";
                //
                //         // Ensure we always have an error and error description.
                //         if (string.IsNullOrEmpty(context.Error))
                //             context.Error = "invalid_token";
                //         if (string.IsNullOrEmpty(context.ErrorDescription))
                //             context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                //
                //         // Add some extra context for expired tokens.
                //         if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() ==
                //             typeof(SecurityTokenExpiredException))
                //         {
                //             var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                //             context.Response.Headers.Add("x-token-expired",
                //                 authenticationException.Expires.ToString("o"));
                //             context.ErrorDescription =
                //                 $"The token expired on {authenticationException.Expires.ToString("o")}";
                //         }
                //
                //         return context.Response.WriteAsync(JsonSerializer.Serialize(new
                //         {
                //             error = context.Error,
                //             error_description = context.ErrorDescription
                //         }));
                //     }
                // };
            });
    }
}