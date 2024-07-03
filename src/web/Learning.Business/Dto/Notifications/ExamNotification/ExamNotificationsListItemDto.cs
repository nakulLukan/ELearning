namespace Learning.Business.Dto.Notifications.ExamNotification;

public class ExamNotificationsListItemDto
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public bool DisplayInHomePage { get; set; }
    public string ImageStaticPath { get; set; }
    public DateOnly? ValidTill { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
