namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class ModelExamOptionDetailResponseDto
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string? OptionText { get; set; }
    public string? OptionImageSignedUrl { get; set; }
    public bool IsCorrectOption { get; set; }
    public int Order { get; set; }
}
