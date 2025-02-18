using Learning.Business.Impl.Data;
using Learning.Shared.Dto;
using Learning.Shared.Dto.DataCollection.ContactUsRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.DataCollection.ContactUsRequest;

public class GetAllContactusQuery : IRequest<PaginatedResponse<GetAllContactUsQueryResponseDto>>
{
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public string? CityFilter { get; set; }
}
public class GetAllContactusQueryHandler : IRequestHandler<GetAllContactusQuery, PaginatedResponse<GetAllContactUsQueryResponseDto>>
{
    private readonly IAppDbContext _appDbContext;

    public GetAllContactusQueryHandler(IAppDbContextFactory appDbContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<GetAllContactUsQueryResponseDto>> Handle(GetAllContactusQuery request, CancellationToken cancellationToken)
    {
        var query = _appDbContext.ContactUsRequests
            .Where(x => string.IsNullOrEmpty(request.CityFilter) || EF.Functions.Like(x.City, $"%{request.CityFilter.ToUpper()}%"));
        var count = await query.CountAsync(cancellationToken);
        var data = await query.OrderByDescending(x => x.CreatedOn)
            .Skip(request.Skip!.Value)
            .Take(request.Take!.Value)
            .Select(x => new GetAllContactUsQueryResponseDto
            {
                City = x.City.ToUpper(),
                ContactNumber = $"{x.CountryCode}{x.PhoneNumber}",
                EmailAddress = x.EmailAddress,
                Name = x.Name.ToUpper(),
                RequestedOn = x.CreatedOn!.Value
            })
            .ToArrayAsync(cancellationToken);
        return new(data, count);
    }
}


