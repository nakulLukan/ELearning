using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.App.Environment.Authorization;

public class IdTokenValidator : IIdentityTokenValidator
{
    public Task<IdentityTokenValidationResult> ValidateAsync(string identityToken, OidcClientOptions options, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new IdentityTokenValidationResult
        {

        });
    }
}
