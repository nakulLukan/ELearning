namespace Learning.Business.Dto.Core.Subject;

public class SubjectSubscriptionDetailDto
{
    public int Id { get; set; }
    public float? DiscountedPrice { get; set; }
    public float? OriginalPrice { get; set; }
    public string SubscriptionType { get; set; }
    public float? DiscountPerc { get; set; }
}
