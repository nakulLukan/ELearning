using Learning.Domain.Master;
using Learning.Shared.Common.Enums;

namespace Learning.Domain.Notification;

public class ModelExamAnswerConfiguration : DomainBase
{
    public int Id { get; set; }
    public string? AnswerText { get; set; }
    public long? AnswerImageId { get; set; }
    public int Order { get; set; }
    public AnswerType AnswerType { get; set; }
    public int QuestionId { get; set; }
    public bool IsCorrectAnswer { get; set; }

    public Attachment? AnswerImage { get; set; }
    public ModelExamQuestionConfiguration? Question { get; set; }
}
