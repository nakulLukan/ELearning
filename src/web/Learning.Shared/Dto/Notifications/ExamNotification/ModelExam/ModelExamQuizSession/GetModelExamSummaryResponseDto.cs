using Learning.Shared.Common.Enums;

namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class GetModelExamSummaryResponseDto
{
    public required int ModelExamId { get; set; }
    public required string ExamName { get; set; }
    public required ModelExamSessionStatusEnum Status { get; set; }
    public required int TotalTimeLimit { get; set; }
    public required int? SessionDurationInSeconds { get; set; }
    public required List<QuestionSummary> QuestionSummary { get; set; }
    public required int? NextModelExamId { get; set; }
    public required string? NextModelExamName { get; set; }

}

public class QuestionSummary
{
    public required int Order { get; set; }
    public required int? SelectedOptionId { get; set; }
    public required bool HasSkipped { get; set; }
    public required string QuestionText { get; set; }
    public required string? QuestionImageUrl { get; set; }
    public required float Score { get; set; }
    public required float NegativeScore { get; set; }
    public required OptionSummary[] OptionSummary { get; set; }
}

public class OptionSummary
{
    public required int OptionId { get; set; }
    public required int Order { get; set; }
    public required string? OptionText { get; set; }
    public required string? OptionImageRelativeUrl { get; set; }
    public required bool? IsCorrectAnswer { get; set; }
}
