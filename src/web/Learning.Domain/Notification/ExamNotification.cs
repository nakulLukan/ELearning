namespace Learning.Domain.Notification;

public class ExamNotification : DomainBase
{
    public int Id { get; set; }
    public required string NotificationTitle { get; set; }
    public required string Descrption { get; set; }
    public bool DisplayInHomePage { get; set; }

    /// <summary>
    /// Unsigned path.
    /// </summary>
    public required string ImageRelativePath { get; set; }

    /// <summary>
    /// If null then exam notification shall have infinite validity
    /// </summary>
    public DateOnly? ValidTill { get; set; }
}
