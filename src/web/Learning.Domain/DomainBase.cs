using System.ComponentModel.DataAnnotations;

namespace Learning.Domain;

public abstract class DomainBase
{
    public DateTimeOffset? CreatedOn { get; set; }

    [MaxLength(36)]
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastUpdatedOn { get; set; }

    [MaxLength(36)]
    public string? LastUpdatedBy { get; set; }
}
