namespace Learning.Business.Dto.Quiz.QuickTest;

public class DefaultQuizConfigResponseDto
{
    public required int? QuizId { get; set; }
    public required float? PassPercentage { get; set; }
    public required int? MaxDiscountPercentage { get; set; }

    public required QuizQuestionConfigResponseDto[]? Questions { get; set; }
}
