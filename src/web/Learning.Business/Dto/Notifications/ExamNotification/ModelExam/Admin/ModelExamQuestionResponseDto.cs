namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;

public class ModelExamQuestionResponseDto
{
    public int Id { get; set; }
    public string? QuestionText { get; set; }
    public string? QuestionImageUrl { get; set; }
    public int Order { get; set; }
    public int Score { get; set; }
    public int ModelExamId { get; set; }
}
