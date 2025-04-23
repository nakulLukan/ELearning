using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.PurchaseHistory;

public class AdminOrderHistoryItemDto
{
    public required long OrderId { get; set; }
    public required string ExamNotificationTitle { get; set; }
    public required float Amount { get; set; }
    public required OrderStatusEnum Status { get; set; }
    public required DateTimeOffset InitiatedOn { get; set; }
    public required DateTimeOffset? CompletedOn { get; set; }
    public required DateTimeOffset? PurchasedOn { get; set; }
    public required DateTimeOffset? ValidTill { get; set; }
    public required string Fullname { get; set; }
    public required string? RzrPayOrderId { get; set; }
}
