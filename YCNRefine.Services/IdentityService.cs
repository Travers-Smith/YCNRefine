using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using YCNRefine.Core.Services;

namespace YCNRefine.Services;

public class IdentityService : IIdentityService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public string? GetEmail()
    {
        return _httpContextAccessor
            ?.HttpContext
            ?.User
            .Claims
            .FirstOrDefault(x => x.Type == "preferred_username")
            ?.Value;
    }

    public string? GetName()
    {
        return _httpContextAccessor
            ?.HttpContext
            ?.User
            .Claims
            .FirstOrDefault(x => x.Type == "name")
            ?.Value;
    }

    public Guid? GetUserIdentifier()
    {
        string? identifier = _httpContextAccessor
            ?.HttpContext
            ?.User
            .Claims
            .FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            ?.Value;

        if (identifier == null)
        {
            return null;
        }

        return Guid.Parse(identifier);
    }

    public bool IsAdmin()
    {
        return _httpContextAccessor
            ?.HttpContext
            ?.User
            ?.Claims
            .Any(x => x.Type == "groups" && x.Value == _configuration["SecurityGroupId"]) ?? false;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor
            ?.HttpContext
            ?.User
            ?.Identity
            ?.IsAuthenticated ?? false;
    }
}
