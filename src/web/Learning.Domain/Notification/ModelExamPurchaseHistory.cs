namespace Learning.Domain.Notification;

public class ModelExamPurchaseHistory
{
    public long Id { get; set; }
    public int ModelExamPackageId { get; set; }
    public DateTime PurchasedOn { get; set; }
    public DateTime ValidTill { get; set; }
    public float Amount { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ModelExamPackage? ModelExamPackage { get; set; }
}
