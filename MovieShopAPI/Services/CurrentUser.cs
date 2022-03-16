using System.Security.Claims;

namespace MovieShopAPI.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;
    
    
    public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    public int UserId => Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value);
    public string Email => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email).Value;
    public string FirstName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GivenName).Value;
    public string LastName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Surname).Value;
    public bool IsAdmin => throw new NotImplementedException();
    public List<string> Roles => throw new NotImplementedException();
    public string IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
}