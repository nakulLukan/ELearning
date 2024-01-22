using Learning.Shared.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.Subject;

public class UpdateSubjectSubscriptionDto
{
    public int SubjectSubscriptionId { get; set; }
    public int SubjectId { get; set; }

    public float? OriginalPrice { get; set; }
    public float? DiscountedPrice { get; set; }

    public SubscriptionExpiryType? SubscriptionType { get; set; }

    public short? NumOfDays { get; set; }

    public DateTime? ExpiryAbsoluteDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
}
