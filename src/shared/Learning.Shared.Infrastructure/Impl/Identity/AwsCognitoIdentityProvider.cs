using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Exceptions.Identity;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Application.Models.Identity;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Models.Identity;
using Learning.Shared.Common.Utilities;
using Microsoft.Extensions.Configuration;

namespace Learning.Shared.Infrastructure.Impl.Identity;

public class AwsCognitoIdentityProvider : IExternalIdentityProvider
{
    private readonly AmazonCognitoIdentityProviderClient _client;
    private readonly string _userPoolId;
    private readonly string _clientId;
    private readonly string _clientSecret;
    public AwsCognitoIdentityProvider(IConfiguration configuration)
    {
        _client = InitialiseAwsClient(configuration);
        _userPoolId = configuration[AppSettingsKeyConstant.ExternalIdentity_Aws_UserPoolId] ?? throw new AppException("User pool is not defined in the environmental variable. Please set the user pool for the key ExternalIdentity_Aws_UserPool");
        _clientId = configuration[AppSettingsKeyConstant.Oidc_ClientId] ?? throw new AppException("Client id is not defined in the environmental variable. Please set the client id for the key Oidc_ClientId");
        _clientSecret = configuration[AppSettingsKeyConstant.Oidc_ClientSecret] ?? throw new AppException("Client id is not defined in the environmental variable. Please set the client id for the key Oidc_ClientId");
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
                    IsEnabled = x.Enabled,
                    IsAccountConfirmed = x.UserStatus == UserStatusType.CONFIRMED
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

    public async Task<SigninResponseDto> Login(string username, string password)
    {
        var authParameters = new Dictionary<string, string>();
        authParameters.Add("USERNAME", username);
        authParameters.Add("PASSWORD", password);

        var authRequest = new InitiateAuthRequest

        {
            ClientId = _clientId,
            AuthParameters = authParameters,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
        };
        try
        {

            var authResponse = await _client.InitiateAuthAsync(authRequest);
            if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
            {
                return await ConfirmNewPassword(authResponse.Session, username, password);
            }

            return new(authResponse.AuthenticationResult.IdToken, authResponse.AuthenticationResult.RefreshToken, authResponse.AuthenticationResult.AccessToken, authResponse.AuthenticationResult.ExpiresIn);
        }
        catch (NotAuthorizedException ex)
        {
            throw new ExternalIdentityProviderException(ExternalIdentityProviderExceptionType.NotAuthorized, ex.Message);
        }
        catch (UserNotConfirmedException)
        {
            throw new ExternalIdentityProviderException(ExternalIdentityProviderExceptionType.AccountNotConfirmed);
        }
        catch (UserNotFoundException)
        {
            throw new ExternalIdentityProviderException(ExternalIdentityProviderExceptionType.AccountNotFound);
        }
    }

    public async Task<SigninResponseDto> ConfirmNewPassword(string session, string username, string newPassword)
    {
        var challengeRequest = new RespondToAuthChallengeRequest
        {
            ChallengeName = ChallengeNameType.NEW_PASSWORD_REQUIRED,
            ClientId = _clientId,
            Session = session, // Retrieve session from previous step
            ChallengeResponses = new Dictionary<string, string>
                {
                    { "USERNAME", username },
                    { "NEW_PASSWORD", newPassword },
                    {"userAttributes.address", "palakkad" },
                    {"userAttributes.name", "nakul" }
                }
        };

        var challengeResponse = await _client.RespondToAuthChallengeAsync(challengeRequest);
        return new(
            challengeResponse.AuthenticationResult.IdToken,
            challengeResponse.AuthenticationResult.RefreshToken,
            challengeResponse.AuthenticationResult.AccessToken,
            challengeResponse.AuthenticationResult.ExpiresIn);
    }

    public async Task SignUpUser(string username, string password, string name, string address)
    {
        var request = new SignUpRequest
        {
            ClientId = _clientId,
            Username = username,
            Password = password,
            UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "phone_number", Value = username },
                    new AttributeType { Name = "address", Value = address},
                    new AttributeType { Name = "name", Value = name },
                },
        };
        try
        {
            var response = await _client.SignUpAsync(request);
        }
        catch (UsernameExistsException)
        {
            throw new ExternalIdentityProviderException(ExternalIdentityProviderExceptionType.UserAlreadyExists);
        }
    }

    public async Task ConfirmUser(string username)
    {
        var confirmSignup = new AdminConfirmSignUpRequest
        {
            UserPoolId = _userPoolId,
            Username = username,
        };
        _ = await _client.AdminConfirmSignUpAsync(confirmSignup);
    }

    public async Task SignOut(string accessToken)
    {
        var signOutRequest = new GlobalSignOutRequest
        {
            AccessToken = accessToken
        };

        _ = await _client.GlobalSignOutAsync(signOutRequest);
    }

    public async Task ConfirmPhoneNumber(string username)
    {
        var request = new AdminUpdateUserAttributesRequest
        {
            UserPoolId = _userPoolId,
            Username = IdentityHelper.ToMobileNumber(username),
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "phone_number_verified", Value = "true" }
            }
        };

        var response = await _client.AdminUpdateUserAttributesAsync(request);
    }

    public async Task<ExternalUser> GetUserDetailsByUsername(string username)
    {
        var request = new AdminGetUserRequest
        {
            UserPoolId = _userPoolId,
            Username = username
        };

        var response = await _client.AdminGetUserAsync(request);
        return new ExternalUser
        {
            CreatedOn = response.UserCreateDate,
            Sub = response.UserAttributes.First(x => x.Name == ClaimConstant.Sub).Value,
            UserName = response.Username,
            LastUpdatedOn = response.UserLastModifiedDate,
            Email = response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.EmailClaim)?.Value ?? string.Empty,
            IsEmailConfirmed = bool.Parse(response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.IsEmailVerifiedClaim)?.Value ?? false.ToString()),
            FullName = response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.Name)?.Value ?? string.Empty,
            PhoneNumber = response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.PhoneNumber)?.Value ?? string.Empty,
            IsPhoneNumberConfirmed = bool.Parse(response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.IsPhoneNumberVerifiedClaim)?.Value ?? false.ToString()),
            Role = response.UserAttributes.FirstOrDefault(x => x.Name == ClaimConstant.AwsRoleClaim)?.Value,
            IsEnabled = response.Enabled,
            IsAccountConfirmed = response.UserStatus == UserStatusType.CONFIRMED
        };
    }

    public async Task ChangeUserPassword(string username, string newPassword)
    {
        var request = new AdminSetUserPasswordRequest
        {
            UserPoolId = _userPoolId,
            Username = username,
            Password = newPassword,
            Permanent = true
        };

        var temp = await _client.AdminSetUserPasswordAsync(request);
    }
}
