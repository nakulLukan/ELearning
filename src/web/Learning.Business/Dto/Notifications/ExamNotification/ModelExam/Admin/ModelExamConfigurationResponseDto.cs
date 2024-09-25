namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class ModelExamConfigurationResponseDto
{
    public required int Id { get; set; }
    public required int ExamNotificationId { get; set; }
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required bool IsFree { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required bool IsActive { get; set; }
    public required string SolutionVideoSignedUrl { get; set; }
    public required int TotalTimeLimitInSeconds { get; set; }
    public required int TotalQuestions { get; set; }
    public required DateTimeOffset CreatedOn { get; set; }
}
