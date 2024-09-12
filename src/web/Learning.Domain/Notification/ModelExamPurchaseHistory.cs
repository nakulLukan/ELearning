namespace Learning.Domain.Notification;

public class ModelExamPurchaseHistory
{
    public long Id { get; set; }
    public int ExamConfigId { get; set; }
    public DateTime PurchasedOn { get; set; }
    public float Amount { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ModelExamConfiguration? ExamConfig { get; set; }
}
