namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class ModelExamDetailResponseDto
{
    public required int Id { get; set; }
    public required int ExamNotificationId { get; set; }
    public required string ExamName { get; set; }
    public required string ExamDescription { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsFree { get; set; }
    public required float Price { get; set; }
    public required float DiscountedPrice { get; set; }
    public required int? Score { get; set; }
    public required int? NegativeScore { get; set; }
    public required string? SolutionVideoMpdFileName { get; set; }
    public required string? SolutionVideoSignedUrl { get; set; }
    public required string? SolutionVideoUploadLink { get; set; }
    public required int TotalTimeLimitInSeconds { get; set; }
    public required DateTimeOffset CreatedOn { get; set; }
    public required ModelExamQuestionMetaData[] Questions { get; set; }
}

public class ModelExamQuestionMetaData
{
    public required int Id { get; set; }
    public required string? QuestionText { get; set; }
    public required string? QuestionImageUrl { get; set; }
    public required int Order { get; set; }
    public required bool IsActive { get; set; }
    public required string OptionNumber { get; set; }
}
