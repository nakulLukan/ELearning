using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Models.Identity;
using Learning.Shared.Common.Utilities;
using Microsoft.Extensions.Configuration;

namespace Learning.Shared.Infrastructure.Impl.Identity;

public class AwsCognitoIdentityProvider : IExternalIdentityProvider
{
    private readonly AmazonCognitoIdentityProviderClient _client;
    private readonly string? _userPoolId;
    public AwsCognitoIdentityProvider(IConfiguration configuration)
    {
        _client = InitialiseAwsClient(configuration);
        _userPoolId = configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_UserPoolId] ?? throw new AppException("User pool is not defined in the environmental variable. Please set the user pool for the key ExternalIdentity_Aws_UserPool");
    }

    private AmazonCognitoIdentityProviderClient InitialiseAwsClient(IConfiguration configuration)
    {
        string accessKey = configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_AccessKey]!;
        string secretKey = configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_SecretKey]!;
        string region = configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_Region]!;
        bool useBasicCredentialAuthentication = bool.Parse(configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_UseBasicCredentialAuthentication] ?? true.ToString());
        if (useBasicCredentialAuthentication)
        {
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            return new AmazonCognitoIdentityProviderClient(credentials, Amazon.RegionEndpoint.GetBySystemName(region));
        }
        else
        {
            return new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(region));
        }
    }

    public async Task<(List<ExternalUser> Users, string? PaginationToken)>
        ListUsersAsync(DateTime? minLastUpdatedOn, string? lastPaginationToken, int? pageSize = 60)
    {
        List<ExternalUser> externalUsers = new();
        string? nextPaginationToken = lastPaginationToken;
        do
        {
            var response = await _client.ListUsersAsync(new ListUsersRequest
            {
                PaginationToken = lastPaginationToken,
                UserPoolId = _userPoolId,
                Limit = pageSize ?? 60
            });

            lastPaginationToken = response.PaginationToken;
            if (lastPaginationToken != null)
            {
                nextPaginationToken = response.PaginationToken;
            }
            externalUsers.AddRange(response.Users
                .Where(x => !minLastUpdatedOn.HasValue || x.UserLastModifiedDate > minLastUpdatedOn.Value)
                .Select(x => new ExternalUser
                {
                    CreatedOn = x.UserCreateDate,
                    Sub = x.Attributes.First(x => x.Name == ClaimConstant.Sub).Value,
                    UserName = x.Username,
                    LastUpdatedOn = x.UserLastModifiedDate,
                    Email = x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.EmailClaim)?.Value ?? string.Empty,
                    IsEmailConfirmed = bool.Parse(x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.IsEmailVerifiedClaim)?.Value ?? false.ToString()),
                    FullName = x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.Name)?.Value ?? string.Empty,
                    PhoneNumber = x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.PhoneNumber)?.Value ?? string.Empty,
                    IsPhoneNumberConfirmed = bool.Parse(x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.IsPhoneNumberVerifiedClaim)?.Value ?? false.ToString()),
                    Role = x.Attributes.FirstOrDefault(x => x.Name == ClaimConstant.AwsRoleClaim)?.Value,
                    IsEnabled = x.Enabled
                }));
        } while (lastPaginationToken != null && (!pageSize.HasValue || externalUsers.Count <= pageSize.Value));

        return (externalUsers, nextPaginationToken);
    }

    public async Task EnableUser(string userId)
    {

        var response = await _client.AdminEnableUserAsync(new AdminEnableUserRequest
        {
            Username = userId,
            UserPoolId = _userPoolId
        });
    }
    public async Task DisableUser(string userId)
    {
        var response = await _client.AdminDisableUserAsync(new AdminDisableUserRequest
        {
            Username = userId,
            UserPoolId = _userPoolId
        });
    }
}
