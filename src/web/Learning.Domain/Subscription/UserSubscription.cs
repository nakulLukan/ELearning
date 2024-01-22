using Learning.Domain.Core;
using Learning.Domain.Identity;

namespace Learning.Domain.Subscription;

public class UserSubscription
{
    public long Id { get; set; }
    public int SubjectId { get; set; }
    public string UserId { get; set; }
    public DateTimeOffset ExpiresOn { get; set; }
    public float Price { get; set; }
    public DateTimeOffset CreatedOn { get; set; }

    public Subject Subject { get; set; }
    public ApplicationUser User { get; set; }
}
