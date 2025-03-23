using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Shared.Common.Constants;
using Learning.Web.Client.DataAnotationValidators;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Learning.Web.Components.Pages.Admin.ConfigureNotification.ExamNotification;

public class AddExamNotificationDto : AddEditExamNotificationRequestDto
{
    [Required]
    [MaxFileSize(FileSizeConstant.ExamNotificationImageFile)]
    public IBrowserFile? ImageFile { get; set; }

    [MaxFileSize(FileSizeConstant.ExamNotificationPdfFile)]
    public IBrowserFile? PdfFile { get; set; }

    [MaxFileSize(FileSizeConstant.ExamNotificationVideoFile)]
    public IBrowserFile? Video { get; set; }
}