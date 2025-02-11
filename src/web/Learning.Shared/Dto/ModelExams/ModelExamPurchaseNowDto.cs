namespace Learning.Shared.Dto.ModelExams;

public class ModelExamPurchaseNowDto
{
    public required int ExamNotificationId { get; set; }
    public required string ExamNotificationName { get; set; }
    public required string ExamNotificationDescription { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required DateOnly ValidUpto { get; set; }
}
