using Learning.Shared.Common.Models.Identity;

namespace Learning.Shared.Application.Contracts.Identity;

public interface IExternalIdentityProvider
{
    public Task<(List<ExternalUser> Users, string? PaginationToken)>
        ListUsersAsync(DateTime? minLastUpdatedOn, string? lastPaginationToken, int? pageSize = 60);


    public Task EnableUser(string userId);
    public Task DisableUser(string userId);
}
