using Learning.Shared.Common.Dto.Storage;

namespace Learning.Web.Components.Pages.Admin.ConfigureNotification.ExamNotification;

internal class AdminExamNotificationDetailDto
{
    public int ExamNotificationId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageAbsPath { get; set; }
    public required string GovtLink { get; set; }
    public string ValidTill { get; set; }
    public string DisplayInHomePage { get; set; }
    public string[] ImportantPoints { get; set; } = [];

    public FileDetailDto? VideoFile { get; set; }
    public FileDetailDto? PdfFile { get; set; }

    public string CreatedOn { get; set; }
    public string LastUpdatedOn { get; set; }
}
