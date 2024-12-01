namespace Learning.Web.Client.Dto.ExamNotifications.ModelExam;

public record struct ModelExamPurchaseDialogParam
{
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required float DiscountedPrice { get; set; }
    public required DateOnly ValidUpto { get; set; }
}
