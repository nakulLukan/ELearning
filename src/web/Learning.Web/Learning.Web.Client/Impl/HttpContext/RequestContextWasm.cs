using Learning.Shared.Contracts.HttpContext;
using Microsoft.AspNetCore.Components.Authorization;

namespace Learning.Web.Client.Impl.HttpContext;

public class RequestContextWasm : RequestContextBase, IRequestContext
{
    public RequestContextWasm(AuthenticationStateProvider authStateProvider) : base(authStateProvider)
    {
    }

    public async Task<string> GetAccessToken()
    {
        return string.Empty;
    }
}
