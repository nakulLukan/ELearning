
using Learning.Shared.Common.Enums;

namespace Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;

public class ModeExamOrderCompleteResponseDto
{
    public required long OrderId { get; set; }
    public required DateTimeOffset CompletedOn { get; set; }
    public required OrderStatusEnum Status { get; set; }
    public required float Amount { get; set; }
}
