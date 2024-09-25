using Learning.Domain.Master;

namespace Learning.Domain.Notification;

public class ModelExamQuestionConfiguration : DomainBase
{
    public int Id { get; set; }
    public string? QuestionText { get; set; }
    public long? QuestionImageId { get; set; }
    public int Order { get; set; }
    public int ExamConfigId { get; set; }
    public int Score { get; set; }
    public bool IsActive { get; set; } = true;

    public Attachment? QuestionImage { get; set; }
    public ModelExamConfiguration? ExamConfig { get; set; }
    public List<ModelExamAnswerConfiguration>? ModelExamAnswers { get; set; }
}
