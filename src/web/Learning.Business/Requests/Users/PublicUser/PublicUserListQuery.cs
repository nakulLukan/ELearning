using Learning.Business.Dto.Users;
using Learning.Business.Impl.Data;
using Learning.Domain.Identity;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Users.PublicUser;

public class PublicUserListQuery : IRequest<PaginatedResponse<PublicUserListItemDto>>
{
    public long? Skip { get; set; }
    public long? Take { get; set; }

    public string SortBy { get; set; }
    public bool IsDescending { get; set; }
    public string EmailOrNameFilter { get; set; }
}

public class PublicUserListQueryHandler : IRequestHandler<PublicUserListQuery, PaginatedResponse<PublicUserListItemDto>>
{
    readonly IAppDbContext _dbContext;

    public PublicUserListQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<PublicUserListItemDto>> Handle(PublicUserListQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _dbContext.AspNetUsers.Where(x => !x.IsAdmin)
            .AsQueryable();
        usersQuery = BuildFilterConditions(request, usersQuery);

        var totalUsers = await usersQuery.CountAsync(cancellationToken);
        usersQuery = BuildBatch(request, usersQuery);
        usersQuery = BuildSortBy(request, usersQuery);
        var users = await usersQuery
            .Select(x => new PublicUserListItemDto
            {
                Id = x.Id,
                AccountCreatedOn = x.AccountCreatedOn.ToHumanizedDateTimeString(),
                ContactNumber = x.OtherDetails.PhoneNumber ?? string.Empty,
                EmailAddress = x.OtherDetails.Email ?? string.Empty,
                IsEmailAddressVerified = x.OtherDetails.EmailConfirmed,
                IsContactNumberVerified = x.OtherDetails.PhoneNumberConfirmed,
                FullName = x.OtherDetails.FullName ?? string.Empty,
                IsActive = x.IsActive,
                Index = x.Index
            })
            .ToListAsync(cancellationToken);
        return new(users, totalUsers);
    }

    private static IQueryable<ApplicationUser> BuildSortBy(PublicUserListQuery request, IQueryable<ApplicationUser> usersQuery)
    {
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            usersQuery = (request.SortBy) switch
            {
                nameof(PublicUserListItemDto.EmailAddress) => usersQuery.SortyBy(x => x.OtherDetails.NormalizedEmail, request.IsDescending),
                nameof(PublicUserListItemDto.IsEmailAddressVerified) => usersQuery.SortyBy(x => x.OtherDetails.EmailConfirmed, request.IsDescending),
                nameof(PublicUserListItemDto.AccountCreatedOn) => usersQuery.SortyBy(x => x.AccountCreatedOn, request.IsDescending),
                nameof(PublicUserListItemDto.Index) => usersQuery.SortyBy(x => x.Index, request.IsDescending),
                nameof(PublicUserListItemDto.FullName) => usersQuery.SortyBy(x => x.OtherDetails.FullName, request.IsDescending),
                _ => throw new NotImplementedException()
            };
        }

        return usersQuery;
    }

    private static IQueryable<ApplicationUser> BuildBatch(PublicUserListQuery request, IQueryable<ApplicationUser> usersQuery)
    {
        if (request.Skip.HasValue)
        {
            usersQuery = usersQuery.Skip((int)request.Skip.Value);
        }

        if (request.Take.HasValue)
        {
            usersQuery = usersQuery.Take((int)request.Take.Value);
        }

        return usersQuery;
    }

    private static IQueryable<ApplicationUser> BuildFilterConditions(PublicUserListQuery request, IQueryable<ApplicationUser> usersQuery)
    {
        if (!string.IsNullOrEmpty(request.EmailOrNameFilter))
        {
            usersQuery = usersQuery
                .Where(x => x.OtherDetails.NormalizedEmail!.Contains(request.EmailOrNameFilter.ToUpper())
                    || (x.OtherDetails.FullName != null
                        && x.OtherDetails.FullName.ToUpper().Contains(request.EmailOrNameFilter.ToUpper())))
                .AsQueryable();
        }

        return usersQuery;
    }
}

