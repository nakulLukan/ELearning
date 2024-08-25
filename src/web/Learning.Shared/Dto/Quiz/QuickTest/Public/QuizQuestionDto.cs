namespace Learning.Shared.Dto.Quiz.QuickTest.Public;

public class QuizQuestionDto
{
    public required int QuestionId { get; set; }
    public required string Question { get; set; }
    public required string? QuestionImageAbsUrl { get; set; }
    public required int Mark { get; set; }
    public required int TimeLimitInSeconds { get; set; }
    public required QuizOptionDto[] Options { get; set; }
}
