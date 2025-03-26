namespace Learning.Web.Client.Dto.ExamNotifications.ModelExam;

public class ModelExamMetaDataDto
{
    public required int ModelExamId { get; set; }
    public required int ExamNotificationId { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required bool IsFree { get; set; }
    public required bool IsLocked { get; set; }
    public required int TotalQuestions{ get; set; }
    public required float TotalMarks { get; set; }
    public required float PositiveMark { get; set; }
    public required float NegativeMark { get; set; }
    public required int TotalTimeInSeconds { get; set; }

}
