namespace Learning.Business.Dto.Quiz.QuickTest;

public class QuizQuestionConfigResponseDto
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required string? QuestionImageAbsUrl { get; set; }
    public required int Order { get; set; }
    public required int Mark { get; set; }
    public required int? TimeLimitInSeconds { get; set; }

    public required QuizQuestionAnswerConfigResponseDto[]? Answers { get; set; }
}

