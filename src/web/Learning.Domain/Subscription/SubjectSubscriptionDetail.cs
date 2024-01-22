using Learning.Domain.Core;
using Learning.Shared.Common.Enums;

namespace Learning.Domain.Subscription;

public class SubjectSubscriptionDetail : DomainBase
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public SubscriptionExpiryType ExpiryType { get; set; }
    public DateTimeOffset? ExpiryAbsoluteDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public short? NumOfDays { get; set; }
    public float DiscountedPrice { get; set; }
    public float OriginalPrice { get; set; }

    public Subject Subject { get; set; }
}
