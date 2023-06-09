using ANY.Identity.Infrastructure.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ANY.Identity.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value;

}