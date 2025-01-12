using Learning.Domain.Identity;
using Learning.Shared.Common.Enums;
namespace Learning.Domain.Notification;

public class ModelExamOrder
{
    public long Id { get; set; }
    public int ModelExamPackageId { get; set; }
    public DateTimeOffset OrderedInitiatedOn { get; set; }
    public DateTimeOffset? OrderedCompletedOn { get; set; }
    public float Amount { get; set; }
    public OrderStatusEnum Status { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ModelExamPackage? ModelExamPackage { get; set; }
    public ApplicationUser? User { get; set; }
}
