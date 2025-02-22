namespace Learning.Shared.Dto.PurchaseHistory;

public class ModelExamPurchaseHistoryItemDto
{
    public required long ModelExamOrderId { get; set; }
    public required int ExamNotificationId { get; set; }
    public required string ExamNotificationName { get; set; }
    public required DateTimeOffset PurchasedOn { get; set; }
    public required float TotalPrice { get; set; }
}
