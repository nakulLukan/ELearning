namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class ModelExamSessionQuestionDetailDto
{
    public required int QuestionId { get; set; }
    public required string QuestionText { get; set; }
    public required string? QuestionImageUrl { get; set; }
    public required int? SelectedOptionId { get; set; }
    public required ModelExamSessionOptionDetailDto[] Options { get; set; }
}

public class ModelExamSessionOptionDetailDto
{
    public required int AnswerId { get; set; }
    public required int Order { get; set; }
    public required string? OptionText { get; set; }
    public required string? OptionImageUrl { get; set; }
}