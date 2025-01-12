namespace Learning.Domain.Notification;

public class ModelExamPurchaseHistory
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public DateTimeOffset PurchasedOn { get; set; }
    public DateTimeOffset ValidTill { get; set; }

    public ModelExamOrder? ModelExamOrder { get; set; }
}
