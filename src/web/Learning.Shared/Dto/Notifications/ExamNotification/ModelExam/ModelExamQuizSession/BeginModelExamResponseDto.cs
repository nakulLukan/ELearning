using Learning.Shared.Common.Enums;

namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class BeginModelExamResponseDto
{
    /// <summary>
    /// Exam session Id
    /// </summary>
    public required long ModelExamResultId { get; set; }

    /// <summary>
    /// When the exam was started on
    /// </summary>
    public required DateTimeOffset StartedOn { get; set; }

    /// <summary>
    /// Total questions in the exam
    /// </summary>
    public required int TotalQuestions { get; set; }

    /// <summary>
    /// Total time allowed for this exam in seconds
    /// </summary>
    public required int TotalTimeInSeconds { get; set; }

    /// <summary>
    /// Current status of this session
    /// </summary>
    public required ModelExamSessionStatusEnum Status { get; set; }

    /// <summary>
    /// Active question for the user. This can be used to load the next question
    /// </summary>
    public required int CurrentQuestionId { get; set; }

    public required int TotalQuestionsAttempted { get; set; }
    public required string ExamName { get; set; }
}
