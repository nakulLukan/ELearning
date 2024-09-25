namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class ModelExamQuestionDetailResponseDto
{
    public required int Id { get; set; }
    public required string? QuestionText { get; set; }
    public required string? QuestionImageSignedUrl { get; set; }
    public required int Score { get; set; }
    public required int Order { get; set; }
    public required bool IsActive { get; set; }
    public required ModelExamOptionDetailResponseDto[] Options { get; set; }
}
