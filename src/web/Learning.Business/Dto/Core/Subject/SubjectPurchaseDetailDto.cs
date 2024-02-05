using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.Core.Subject;

public class SubjectPurchaseDetailDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string CourseName { get; set; }
    public string ClassName { get; set; }

    public float BuyPrice { get; set; }
    public string SubscriptionEndDate { get; set; }

    public SubjectPurchaseValidationEnum Result { get; set; }
}
