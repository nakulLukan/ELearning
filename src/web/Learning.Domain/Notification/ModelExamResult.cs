using Learning.Shared.Common.Enums;

namespace Learning.Domain.Notification;

public class ModelExamResult : DomainBase
{
    public long Id { get; set; }
    public int ExamConfigId { get; set; }
    public DateTimeOffset StartedOn { get; set; }
    public DateTimeOffset? CompletedOn { get; set; }
    public ModelExamSessionStatusEnum Status { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ModelExamConfiguration? ExamConfig { get; set; }
    public List<ModelExamResultDetail>? ModelExamResultDetails { get; set; }
}
