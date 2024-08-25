using Learning.Shared.Enums;

namespace Learning.Web.Client.Dto.Quiz;

public class QuizLocalStorageModel
{
    public QuizAttempStatusEnum Status { get; set; }
    public int MarkScored { get; set; }
    public int TotalMark { get; set; }
    public int QuizVersionNumber { get; set; }
    public int CurrentQuestionNumber { get; set; }
    public int TotalDiscount { get; set; }
    public string? DiscountCode { get; set; }
}
