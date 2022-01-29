using System.Security.Claims;
using voonex.api.Controllers;

namespace voonex.api.Services;

public class UserInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string SessionToken => _httpContextAccessor.HttpContext!.User.Claims
        .First(x => x.Type == AuthController.ClaimTypeSessionToken).Value;
    public Guid UserId => Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims
        .First(x => x.Type == ClaimTypes.Sid).Value);
    public string Email => _httpContextAccessor.HttpContext!.User.Claims
        .First(x => x.Type == ClaimTypes.Email).Value;
    public string Name => _httpContextAccessor.HttpContext!.User.Claims
        .First(x => x.Type == ClaimTypes.Name).Value;
}