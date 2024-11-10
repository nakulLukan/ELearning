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
        throw new NotImplementedException();
    }

    public Task<string> GetName()
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserId()
    {
        return Task.FromResult(_contextAccessor!.HttpContext!.User!.Claims.First(x => x.Type == ClaimConstant.AwsUserNameClaim).Value);
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
