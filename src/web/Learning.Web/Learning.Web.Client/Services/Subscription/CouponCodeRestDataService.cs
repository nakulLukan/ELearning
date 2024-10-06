using FluentResults;
using Learning.Web.Client.Contracts.Services.Subscription;
using Learning.Web.Client.Services.WebServices;

namespace Learning.Web.Client.Services.Subscription;

public class CouponCodeRestDataService : ICouponCodeDataService
{
    private readonly IPublicQuizHttpClient _httpClient;

    public CouponCodeRestDataService(IPublicQuizHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<long>> SaveCouponCode(string couponCode, float discountPercentage)
    {
        try
        {
            var result = await _httpClient.SaveCouponCode(new Shared.Dto.Subscription.Offer.AddCouponCodeCommandRequestDto()
            {
                CouponCode = couponCode,
                DiscountPercentage = discountPercentage
            });

            return Result.Ok(result.Data);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
