namespace Learning.Shared.Dto.Notifications.ExamNotification;

public class ActiveExamNotificationDetailDto
{
    public int ExamNotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageAbsPath { get; set; }
    public required string GovtLink { get; set; }
    public DateOnly? ValidTill { get; set; }
    public string[]? ImportantPoints { get; set; }

    public required string VideoAbsUrl { get; set; }
    public string? PdfFileAbsUrl { get; set; }
}
