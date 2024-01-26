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
        usersQuery = BuildBatch(request, usersQuery);

        var totalUsers = await usersQuery.CountAsync(cancellationToken);
        usersQuery = BuildSortBy(request, usersQuery);
        var users = await usersQuery
            .Select(x => new PublicUserListItemDto
            {
                Id = x.Id,
                AccountCreatedOn = x.AccountCreatedOn.ToString(),
                ContactNumber = x.OtherDetails.PhoneNumber,
                EmailAddress = x.Email,
                IsEmailAddressVerified = x.EmailConfirmed,
                IsContactNumberVerified = x.PhoneNumberConfirmed,
                FirstName = x.OtherDetails.FirstName,
                LastName = x.OtherDetails.LastName,
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
                nameof(PublicUserListItemDto.EmailAddress) => usersQuery.SortyBy(x => x.NormalizedEmail, request.IsDescending),
                nameof(PublicUserListItemDto.IsEmailAddressVerified) => usersQuery.SortyBy(x => x.EmailConfirmed, request.IsDescending),
                nameof(PublicUserListItemDto.AccountCreatedOn) => usersQuery.SortyBy(x => x.AccountCreatedOn, request.IsDescending),
                nameof(PublicUserListItemDto.Index) => usersQuery.SortyBy(x => x.Index, request.IsDescending),
                nameof(PublicUserListItemDto.FirstName) => usersQuery.SortyBy(x => x.OtherDetails.FirstName, request.IsDescending),
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
                .Where(x => x.NormalizedEmail.Contains(request.EmailOrNameFilter.ToUpper())
                    || (x.OtherDetails.FullName != null
                        && x.OtherDetails.FullName.ToUpper().Contains(request.EmailOrNameFilter.ToUpper())))
                .AsQueryable();
        }

        return usersQuery;
    }
}

