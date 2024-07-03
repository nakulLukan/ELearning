namespace Learning.Business.Dto.Notifications.ExamNotification;

public class ActiveExamNotificationCacheDto
{
    public int NotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageRelativePath { get; set; }
}
