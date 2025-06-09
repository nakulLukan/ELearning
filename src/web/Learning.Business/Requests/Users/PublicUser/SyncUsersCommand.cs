using Learning.Business.Contracts.Services.Master;
using Learning.Business.Dto.Users;
using Learning.Business.Impl.Data;
using Learning.Domain.Identity;
using Learning.Shared.Application.Contracts.Identity;
using Learning.Shared.Application.Helpers;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Models.Identity;
using Learning.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Users.PublicUser;

public class SyncUsersCommand : IRequest<SyncUsersResponseDto>
{
}

public class SyncUsersCommandHandler : IRequestHandler<SyncUsersCommand, SyncUsersResponseDto>
{
    private readonly IExternalIdentityProvider _identityProvider;
    private readonly IAppDbContext _appDbContext;
    private readonly IAppMasterSettingManager _masterSettingsManager;

    public SyncUsersCommandHandler(
        IExternalIdentityProvider identityProvider,
        IAppDbContextFactory appDbContext,
        IAppMasterSettingManager masterSettingsManager)
    {
        _identityProvider = identityProvider;
        _appDbContext = appDbContext.CreateDbContext();
        _masterSettingsManager = masterSettingsManager;
    }

    public async Task<SyncUsersResponseDto> Handle(SyncUsersCommand request, CancellationToken cancellationToken)
    {
        var lastPagination = await _masterSettingsManager.GetValue<string?>(_appDbContext, AppMasterSettingKeyConstant.COG_USER_LIST_PAGINATION);
        var minDate = await _masterSettingsManager.GetValue<DateTime?>(_appDbContext, AppMasterSettingKeyConstant.COG_USER_MIN_DATE);
        (List<ExternalUser> users, string? nextPaginationToken) = await _identityProvider.ListUsersAsync(minDate, lastPagination);
        Dictionary<string, int> roleMapper = await CreateRoles(users, cancellationToken);

        var existingUserIds = await UpdateExistingUsers(users, roleMapper, cancellationToken);
        int newUsersCreated = await AddNewUsers(users, existingUserIds, roleMapper, cancellationToken);

        if (users.Any())
        {
            minDate = DateTime.UtcNow;
            await _masterSettingsManager.SetValue(_appDbContext, AppMasterSettingKeyConstant.COG_USER_MIN_DATE, minDate.Value, cancellationToken);
            await _masterSettingsManager.SetValue(_appDbContext, AppMasterSettingKeyConstant.COG_USER_LIST_PAGINATION, nextPaginationToken, cancellationToken);
        }
        return new(newUsersCreated, existingUserIds.Count);
    }

    private async Task<Dictionary<string, int>> CreateRoles(List<ExternalUser> users, CancellationToken cancellationToken)
    {
        var distinctRoles = users.Select(x => x.Role).Distinct();
        var existingRoles = await _appDbContext.AppRoles.ToListAsync();
        foreach (var newRole in distinctRoles.Where(x => x != null && !existingRoles.Select(x => x.RoleName).Contains(x)))
        {
            var appRole = new AppRole
            {
                RoleName = newRole!,
            };

            _appDbContext.AppRoles.Add(appRole);
            existingRoles.Add(appRole);
        }

        await _appDbContext.SaveAsync(cancellationToken);
        var roleMapper = existingRoles.ToDictionary(x => x.RoleName, x => x.Id);
        return roleMapper;
    }

    private async Task<int> AddNewUsers(List<ExternalUser> users, HashSet<string> existingUserIds, Dictionary<string, int> roleMapper, CancellationToken cancellationToken)
    {
        int accountsCreated = 0;
        foreach (var newUser in users.Where(x => !existingUserIds.Contains(x.Sub)))
        {
            _appDbContext.AspNetUsers.Add(new()
            {
                Id = newUser.Sub,
                AccountCreatedOn = newUser.CreatedOn,
                IsActive = true,
                IsAdmin = IdentityHelper.IsAdminUser(newUser.Role),
                OtherDetails = new()
                {
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    EmailConfirmed = newUser.IsEmailConfirmed,
                    PhoneNumberConfirmed = newUser.IsPhoneNumberConfirmed,
                    FullName = newUser.FullName,
                    NormalizedEmail = newUser.Email.ToNormalizedString(),
                    Place = newUser.Place
                },
                RoleId = roleMapper.ContainsKey(newUser.Role ?? string.Empty) ? roleMapper[newUser.Role] : null,
            });
            accountsCreated++;
        }

        await _appDbContext.SaveAsync(cancellationToken);
        return accountsCreated;
    }

    private async Task<HashSet<string>> UpdateExistingUsers(List<ExternalUser> users, Dictionary<string, int> roleMapper, CancellationToken cancellationToken)
    {
        int pageSize = 100;
        int pageNumber = 0;
        int currentBatchCount = 0;
        HashSet<string> existingUserIds = new HashSet<string>();
        do
        {
            var existingUsers = await _appDbContext.AspNetUsers.AsTracking()
            .Where(x => users.Select(x => x.Sub).Contains(x.Id))
            .Include(x => x.OtherDetails)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
            currentBatchCount = existingUsers.Count;

            foreach (var existingUser in existingUsers)
            {
                var user = users.First(x => x.Sub == existingUser.Id);
                existingUserIds.Add(existingUser.Id);
                // Public users wont have a role

                if (existingUser.IsAdmin != IdentityHelper.IsAdminUser(user.Role))
                {
                    existingUser.IsAdmin = !existingUser.IsAdmin;
                }

                if (existingUser.IsActive != user.IsEnabled)
                {
                    existingUser.IsActive = user.IsEnabled;
                }

                if (existingUser.OtherDetails!.EmailConfirmed != user.IsEmailConfirmed)
                {
                    existingUser.OtherDetails.EmailConfirmed = user.IsEmailConfirmed;
                }

                if (existingUser.OtherDetails.PhoneNumberConfirmed != user.IsPhoneNumberConfirmed)
                {
                    existingUser.OtherDetails.PhoneNumberConfirmed = user.IsPhoneNumberConfirmed;
                }

                if (existingUser.OtherDetails.FullName != user.FullName)
                {
                    existingUser.OtherDetails.FullName = user.FullName;
                }

                if (existingUser.OtherDetails.Place != user.Place.ToUpper())
                {
                    existingUser.OtherDetails.Place = user.Place.TrimToLen(DomainConstant.PlaceFieldMaxLength).ToUpper();
                }

                if (roleMapper.ContainsKey(user.Role ?? string.Empty))
                {
                    if (existingUser.RoleId != roleMapper[user.Role ?? string.Empty])
                    {
                        existingUser.RoleId = roleMapper[user.Role ?? string.Empty];
                    }
                }
                else
                {
                    if (existingUser.RoleId.HasValue)
                    {
                        existingUser.RoleId = null;
                    }
                }
            }

            await _appDbContext.SaveAsync(cancellationToken);
        }
        while (currentBatchCount == pageSize);
        return existingUserIds;
    }
}

