using Learning.Core.Contracts.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Core.PageModels.AccountManagement;

public class LoginPageModel : PageModelBase
{
    private readonly ISessionManager _sessionManager;

    public LoginPageModel(ILogger<LoginPageModel> logger, ISessionManager sessionManager) : base(logger)
    {
        _sessionManager = sessionManager;
    }

    public override async Task InitializeAsync()
    {
        await _sessionManager.Login();
    }
}
