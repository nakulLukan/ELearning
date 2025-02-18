namespace Learning.Shared.Dto.Subscription.Offer;

public class GetAllCouponCodeResponseDto
{
    public required long Id { get; set; }
    public required string CouponCode { get; set; }
    public required float Discount { get; set; }
    public required DateTimeOffset CreatedOn { get; set; }
    public required DateTimeOffset? ExpiresOn { get; set; }
    public required DateTimeOffset? UsedOn { get; set; }
    public required bool IsUsed { get; set; }
}
