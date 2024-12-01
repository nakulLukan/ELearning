namespace Learning.Web.Client.Dto.ExamNotifications.ModelExam;

public class ModelExamMetaDataDto
{
    public required int ModelExamId { get; set; }
    public required int ExamNotificationId { get; set; }
    public required int ExamConfigId { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required bool IsFree { get; set; }
    public required bool IsLocked { get; set; }
    public required DateOnly ValidUpto { get; set; }
}
