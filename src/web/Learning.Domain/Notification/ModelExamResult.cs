namespace Learning.Domain.Notification;

public class ModelExamResult : DomainBase
{
    public long Id { get; set; }
    public int ExamConfigId { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime CompletedOn { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ModelExamConfiguration? ExamConfig { get; set; }
    public List<ModelExamResultDetail>? ModelExamResultDetails { get; set; }
}
