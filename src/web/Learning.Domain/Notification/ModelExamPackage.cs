namespace Learning.Domain.Notification;

public class ModelExamPackage
{
    public int Id { get; set; }
    public float Price { get; set; }
    public float DiscountedPrice { get; set; }
    public int ExamNotificationId { get; set; }

    public ExamNotification? ExamNotification { get; set; }
    public List<ModelExamConfiguration>? ModelExamConfigs { get; set; }
    public List<ModelExamPurchaseHistory>? PurchaseHitories { get; set; }
}
