using Learning.Domain.Content;
using Learning.Domain.Master;

namespace Learning.Domain.Core;

public class Lesson : DomainBase
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int? VideoId { get; set; }
    public int Order { get; set; }
    public int? ChapterId { get; set; }

    /// <summary>
    /// Flag to indicate whether this lesson can be watched without subscription.
    /// </summary>
    public bool IsPreviewable { get; set; }

    public Chapter Chapter { get; set; }
    public Video Video { get; set; }
}
