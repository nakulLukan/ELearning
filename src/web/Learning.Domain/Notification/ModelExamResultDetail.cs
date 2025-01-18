namespace Learning.Domain.Notification;

public class ModelExamResultDetail
{
    public long Id { get; set; }
    public long ModelExamResultId { get; set; }
    public int QuestionId { get; set; }
    public int? SelectedAnswerId { get; set; }
    public bool HasSkipped { get; set; }

    public ModelExamQuestionConfiguration? Question { get; set; }
    public ModelExamAnswerConfiguration? SelectedAnswer { get; set; }
}
