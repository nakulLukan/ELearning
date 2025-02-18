using Learning.Business.Impl.Data;
using Learning.Shared.Dto;
using Learning.Shared.Dto.Subscription.Offer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Subscription.Offer;

public class GetAllCouponCodeQuery : IRequest<PaginatedResponse<GetAllCouponCodeResponseDto>>
{
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public string? CouponCodeFilter { get; set; }
}

public class GetAllCouponCodeQueryHandler : IRequestHandler<GetAllCouponCodeQuery, PaginatedResponse<GetAllCouponCodeResponseDto>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCouponCodeQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<GetAllCouponCodeResponseDto>> Handle(GetAllCouponCodeQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.CouponCodes
            .Where(x => string.IsNullOrEmpty(request.CouponCodeFilter) || x.Code == request.CouponCodeFilter);
        var couponCodeCount = await query.CountAsync(cancellationToken);
        var couponCodes = await query
            .OrderByDescending(x => x.CouponCreatedOn)
            .Skip(request.Skip!.Value)
            .Take(request.Take!.Value)
            .Select(x => new GetAllCouponCodeResponseDto()
            {
                Id = x.Id,
                CouponCode = x.Code,
                CreatedOn = x.CouponCreatedOn,
                Discount = x.DiscountPercentage,
                UsedOn = x.CouponUsedOn,
                IsUsed = x.IsUsed,
                ExpiresOn = x.ExpiresOn
            })
            .ToArrayAsync(cancellationToken);
        return new(couponCodes, couponCodeCount);
    }
}
