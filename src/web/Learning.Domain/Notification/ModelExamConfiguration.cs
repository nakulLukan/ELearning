using Learning.Domain.Master;

namespace Learning.Domain.Notification;

public class ModelExamConfiguration : DomainBase
{
    public int Id { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFree { get; set; }
    public bool IsActive { get; set; }
    public long? ExamSolutionVideoId { get; set; }
    
    /// <summary>
    /// Score per question
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// Negative score for wrong answer
    /// </summary>
    public int NegativeScore { get; set; }

    /// <summary>
    /// Value is seconds
    /// </summary>
    public int TotalTimeLimit { get; set; }
    public int ModelExamPackageId { get; set; }
    public int ExamNotificationId { get; set; }

    public Attachment? ExamSolutionVideo { get; set; }
    public ExamNotification? ExamNotification { get; set; }
    public ModelExamPackage? ModelExamPackage { get; set; }

    public List<ModelExamQuestionConfiguration>? Questions { get; set; }
}
