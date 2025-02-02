using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Payment;

public class ModelExamOrderStepDetailDto
{
    public required long ModelExamOrderId { get; set; }
    public required string? RazorpayOrderId { get; set; }
    public required float Amount { get; set; }
    public required OrderStatusEnum Status { get; set; }
}
