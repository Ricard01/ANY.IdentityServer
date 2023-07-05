using System.Security.Claims;
using ANY.DuendeIDS.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ANY.DuendeIDS.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        Log.Information("httpContextAccessor {Http}", _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
    public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
    // public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value;

}