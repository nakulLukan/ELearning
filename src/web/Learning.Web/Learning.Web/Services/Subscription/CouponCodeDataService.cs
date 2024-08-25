using FluentResults;
using Learning.Business.Requests.Subscription.Offer;
using Learning.Web.Client.Contracts.Services.Subscription;
using MediatR;

namespace Learning.Web.Services.Subscription;

public class CouponCodeDataService : ICouponCodeDataService
{
    private readonly IMediator _mediator;
    public CouponCodeDataService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<Result<long>> SaveCouponCode(string couponCode, float discountPercentage)
    {
        try
        {
            var couponId = await _mediator.Send(new AddCouponCodeCommand()
            {
                CouponCode = couponCode,
                DiscountPercentage = discountPercentage
            });
            return couponId.Data;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
