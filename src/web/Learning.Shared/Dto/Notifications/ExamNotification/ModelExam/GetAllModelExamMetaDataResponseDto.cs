namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;

public class GetAllModelExamMetaDataResponseDto
{
    public required int ExamNotificationId { get; set; }
    public required int ExamConfigId { get; set; }
    public required string ExamName { get; set; } = string.Empty;
    public required string ExamDescription { get; set; } = string.Empty;
    public required bool IsLocked { get; set; }
    public required bool IsFree { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required DateOnly ValidUpto { get; set; }
    public required int TotalQuestions { get; set; }
    public required int TotalTimeInSeconds { get; set; }
}
