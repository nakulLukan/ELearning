using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Payment;

public class ModelExamOrderStepDetailDto
{
    public required long ModelExamOrderId { get; set; }
    public required string? RazorpayOrderId { get; set; }
    public required double AmountInPaisa { get; set; }
    public required OrderStatusEnum Status { get; set; }

    public required string? Name { get; set; }
    public required string? Email { get; set; }
    public required string? PhoneNumber { get; set; }

    public required string? NotificationTitle { get; set; }
    public required DateTimeOffset? OrderCompletedOn { get; set; }
    public required DateTimeOffset? Validity { get; set; }
    public required int TotalPaidExamsInPackage { get; set; }
    public required int ModelExamId { get; set; }
    public required int ExamNotificationId { get; set; }
}
