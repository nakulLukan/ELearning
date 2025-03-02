using Learning.Business.Contracts.HttpContext;
using Learning.Shared.Common.Constants;

namespace Learning.Web.Impl.HttpContext;

public class ApiRequestContext : IApiRequestContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    public ApiRequestContext(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public Task<string> GetAccessToken()
    {
        return Task.FromResult(_contextAccessor!.HttpContext!.Items.ContainsKey("access_token") ? (string)_contextAccessor.HttpContext.Items["access_token"]! : string.Empty);
    }

    public Task<string?> GetEmail()
    {
        return Task.FromResult(_contextAccessor!.HttpContext!.User!.Claims.FirstOrDefault(x => x.Type == ClaimConstant.EmailClaim)?.Value);
    }

    public Task<string> GetName()
    {
        return Task.FromResult(_contextAccessor!.HttpContext!.User!.Claims.First(x => x.Type == ClaimConstant.Name).Value);
    }

    public Task<string> GetPhoneNumber()
    {
        return Task.FromResult(_contextAccessor!.HttpContext!.User!.Claims.First(x => x.Type == ClaimConstant.PhoneNumber).Value);
    }

    public async Task<string> GetUserId()
    {
        if (!await IsAuthenticated()) return string.Empty;
        return _contextAccessor!.HttpContext!.User!.Claims.First(x => x.Type == ClaimConstant.Sub).Value;
    }

    public Task<string> GetUserRole()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAdmin()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAuthenticated()
    {
        return Task.FromResult(_contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false);
    }

    public Task<(bool IsAuthenticated, bool IsAdmin)> IsLoggedIn()
    {
        throw new NotImplementedException();
    }
}
