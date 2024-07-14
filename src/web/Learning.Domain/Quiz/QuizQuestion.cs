namespace Learning.Domain.Quiz;

public class QuizQuestion
{
    public required int Id { get; set; }
    public required string Question { get; set; }
    public required int Order { get; set; }
    public required int QuizConfigurationId { get; set; }
    public required int Mark { get; set; }
    public required int TimeLimitInSeconds { get; set; }

    public QuizConfiguration? QuizConfiguration { get; set; }
    public List<QuizQuestionAnswer>? Answers { get; set; }
}
