namespace Learning.Shared.Dto.Notifications.ExamNotification;

public class ActiveExamNotificationCacheDto
{
    public required int NotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageRelativePath { get; set; }
    public required DateOnly? ValidTill { get; set; }
    public required bool DisplayInHomepage  { get; set; }
}
