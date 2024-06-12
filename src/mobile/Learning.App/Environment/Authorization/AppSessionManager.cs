using IdentityModel.OidcClient;
using Learning.Core.Contracts.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.App.Environment.Authorization;

public class AppSessionManager : ISessionManager
{
    private readonly ILogger<AppSessionManager> _logger;
    private readonly OidcClient _oidcClient;

    public AppSessionManager(ILogger<AppSessionManager> logger, OidcClient oidcClient)
    {
        _logger = logger;
        _oidcClient = oidcClient;
    }

    public async Task<string> Login()
    {
        try
        {
            var loginResult = await _oidcClient.LoginAsync(new LoginRequest()
            {
                FrontChannelExtraParameters = new IdentityModel.Client.Parameters([new KeyValuePair<string, string>("deviceId", "xyz")])
            });
            return "";
        }
        catch (Exception ex)
        {
        }
        return "";
    }
}
