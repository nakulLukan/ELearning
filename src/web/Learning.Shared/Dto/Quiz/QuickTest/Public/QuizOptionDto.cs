namespace Learning.Shared.Dto.Quiz.QuickTest.Public;

public class QuizOptionDto
{
    public required int OptionId { get; set; }
    public required int Order { get; set; }
    public required string? OptionText { get; set; }
    public required string? OptionImageAbsUrl { get; set; }
    public required bool IsCorrectOption { get; set; }
}
