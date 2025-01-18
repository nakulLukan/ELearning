namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class SubmitExamQuestionResponseRequestDto
{
    public required int QuestionId { get; set; }
    public required int? SelectedAnswerId { get; set; }
    public required bool HasSkipped { get; set; }
}
