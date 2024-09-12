using Learning.Domain.Master;
using Learning.Shared.Common.Enums;

namespace Learning.Domain.Quiz;

public class QuizQuestionAnswer
{
    public required int Id { get; set; }
    public required string? AnswerText { get; set; }
    public required long? AnswerImageId { get; set; }
    public required int Order { get; set; }
    public required AnswerType AnswerType { get; set; }
    public required int QuizQuestionId { get; set; }
    public required bool IsCorrectAnswer { get; set; }

    public QuizQuestion? QuizQuestion { get; set; }
    public Attachment? AnswerImage { get; set; }
}
