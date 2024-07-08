namespace Learning.Business.Dto.Notifications.ExamNotification;

public class ExamNotificationCardItemDto
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageAbsPath { get; set; }
    public required DateOnly? ValidTill { get; set; }
}
