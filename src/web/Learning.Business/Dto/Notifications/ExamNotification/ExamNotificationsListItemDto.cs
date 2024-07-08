namespace Learning.Business.Dto.Notifications.ExamNotification;

public class ExamNotificationsListItemDto
{
    public int Id { get; set; }
    public int Index { get; set; }
    public required string Title { get; set; }
    public required bool DisplayInHomePage { get; set; }
    public required string ImageStaticPath { get; set; }
    public required DateOnly? ValidTill { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}
