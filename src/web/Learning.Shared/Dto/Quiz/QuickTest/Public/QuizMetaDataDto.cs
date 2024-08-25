namespace Learning.Shared.Dto.Quiz.QuickTest.Public;

public class QuizMetaDataDto
{
    public required int QuizConfigId { get; set; }
    public required int TotalQuestions { get; set; }
    public required int TotalMarks { get; set; }
    public required int TotalTimeInSeconds { get; set; }
    public required float MinimumPassPercentage { get; set; }
    public required int TotalDiscount { get; set; }
    public required int QuizVersionNumber { get; set; }
}
