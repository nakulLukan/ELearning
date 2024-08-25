using Learning.Business.Impl.Data;
using Learning.Domain.Subscription.Offer;
using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.Subscription.Offer;
using MediatR;

namespace Learning.Business.Requests.Subscription.Offer;

public class AddCouponCodeCommand : AddCouponCodeCommandRequestDto, IRequest<ResponseDto<long>>
{
}

public class AddCouponCodeCommandHandler : IRequestHandler<AddCouponCodeCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _dbContext;

    public AddCouponCodeCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ResponseDto<long>> Handle(AddCouponCodeCommand request, CancellationToken cancellationToken)
    {
        var couponCode = new CouponCode
        {
            Id = 0,
            Code = request.CouponCode,
            CouponCreatedOn = DateTimeOffset.UtcNow,
            CouponUsedOn = DateTimeOffset.UtcNow,
            DiscountPercentage = request.DiscountPercentage,
            ExpiresOn = null,
            IsUsed = false
        };

        _dbContext.CouponCodes.Add(couponCode);
        await _dbContext.SaveAsync(cancellationToken);
        return new(couponCode.Id);
    }
}
