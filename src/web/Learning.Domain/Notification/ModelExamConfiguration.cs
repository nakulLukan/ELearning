using Learning.Domain.Master;

namespace Learning.Domain.Notification;

public class ModelExamConfiguration
{
    public int Id { get; set; }
    public string ExamName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFree { get; set; }
    public float Price { get; set; }
    public float DiscountedPrice { get; set; }
    public bool IsActive { get; set; }
    public long ExamSolutionVideoId { get; set; }

    /// <summary>
    /// Value is seconds
    /// </summary>
    public int TotalTimeLimit { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime LastUpdatedOn { get; set; }
    public int Order { get; set; }
    public int ExamNotificationId { get; set; }

    public Attachment? ExamSolutionVideo { get; set; }
    public ExamNotification? ExamNotification { get; set; }
    public List<ModelExamQuestionConfiguration>? Questions { get; set; }
}
