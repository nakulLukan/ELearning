using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.Quiz.QuickTest;

public class QuizQuestionAnswerConfigResponseDto
{
    public required int Id { get; set; }
    public required string? AnswerText { get; set; }
    public required string? AnswerImageAbsUrl { get; set; }
    public required QuizAnswerType AnswerType { get; set; }
    public required bool IsCorrectAnswer { get; set; }
    public required int Order { get; set; }
}

