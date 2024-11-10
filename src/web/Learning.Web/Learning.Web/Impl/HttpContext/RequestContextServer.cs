using Learning.Shared.Contracts.HttpContext;
using Learning.Web.Client.Impl.HttpContext;
using Microsoft.AspNetCore.Components.Authorization;

namespace Learning.Web.Impl.HttpContext;

public class RequestContextServer : RequestContextBase, IRequestContext
{
    public RequestContextServer(AuthenticationStateProvider authStateProvider) : base(authStateProvider)
    {
    }

    public Task<string> GetAccessToken()
    {
        return Task.FromResult(string.Empty);
    }
}
