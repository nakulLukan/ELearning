using Learning.Domain.Master;

namespace Learning.Domain.Notification;

public class ExamNotification : DomainBase
{
    public int Id { get; set; }
    public required string NotificationTitle { get; set; }
    public required string Description { get; set; }
    public bool DisplayInHomePage { get; set; }
    public string? ImportantPoints { get; set; }
    public required string GovtLink { get; set; }
    public long? PdfFileId { get; set; }
    public long? VideoId { get; set; }

    /// <summary>
    /// Unsigned path.
    /// </summary>
    public required string ImageRelativePath { get; set; }

    /// <summary>
    /// If null then exam notification shall have infinite validity
    /// </summary>
    public DateOnly? ValidTill { get; set; }

    public Attachment PdfFile { get; set; }
    public Attachment Video { get; set; }
}
