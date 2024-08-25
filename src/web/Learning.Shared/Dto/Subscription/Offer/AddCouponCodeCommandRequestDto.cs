namespace Learning.Shared.Dto.Subscription.Offer;

public class AddCouponCodeCommandRequestDto
{
    public required string CouponCode { get; set; }
    public required float DiscountPercentage { get; set; }
}
