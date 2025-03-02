using Learning.Shared.Application.Models.Identity;
using Learning.Shared.Common.Models.Identity;

namespace Learning.Shared.Application.Contracts.Identity;

public interface IExternalIdentityProvider
{
    public Task<(List<ExternalUser> Users, string? PaginationToken)>
        ListUsersAsync(DateTime? minLastUpdatedOn, string? lastPaginationToken, int? pageSize = 60);

    public Task EnableUser(string userId);
    public Task DisableUser(string userId);
    public Task<SigninResponseDto> Login(string username, string password);
    public Task SignUpUser(string username, string password, string name, string address);

    public Task ConfirmUser(string username);
    public Task ConfirmPhoneNumber(string username);

    public Task SignOut(string accessToken);

    public Task<ExternalUser> GetUserDetailsByUsername(string username);
    public Task ChangeUserPassword(string username, string newPassword);

}
