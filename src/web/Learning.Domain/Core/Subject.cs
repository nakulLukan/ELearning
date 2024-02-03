using Learning.Domain.Master;
using Learning.Domain.Subscription;

namespace Learning.Domain.Core;

public class Subject : DomainBase
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? ClassId { get; set; }
    public int? SubjectGroupLookupId { get; set; }

    /// <summary>
    /// Relative path and filename of the thumbnail image.
    /// </summary>
    public string ThumbnailRelativePath { get; set; }

    public ClassDivision Class { get; set; }
    public List<Chapter> Chapters { get; set; }
    public LookupValue SubjectGroupLookup { get; set; }
    public SubjectSubscriptionDetail SubscriptionDetail { get; set; }
}
