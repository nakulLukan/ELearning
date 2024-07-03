using Learning.Shared.Common.Dto.Storage;

namespace Learning.Business.Dto.Notifications.ExamNotification;

public class AdminExamNotificationDetailResponseDto
{
    public int ExamNotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageAbsPath { get; set; }
    public required string GovtLink { get; set; }
    public DateOnly? ValidTill { get; set; }
    public bool DisplayInHomePage { get; set; }
    public string? ImportantPoints { get; set; }

    public FileDetailDto? VideoFile { get; set; }
    public FileDetailDto? PdfFile { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset LastUpdatedOn { get; set; }
}
