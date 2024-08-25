using FluentResults;

namespace Learning.Web.Client.Contracts.Services.Subscription;

public interface ICouponCodeDataService
{
    public Task<Result<long>> SaveCouponCode(string couponCode, float discountPercentage);
}
