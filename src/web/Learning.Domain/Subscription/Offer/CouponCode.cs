namespace Learning.Domain.Subscription.Offer;

public class CouponCode
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public float DiscountPercentage { get; set; }
    public bool IsUsed { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }
    public DateTimeOffset CouponCreatedOn { get; set; }
    public DateTimeOffset CouponUsedOn { get; set; }
}
