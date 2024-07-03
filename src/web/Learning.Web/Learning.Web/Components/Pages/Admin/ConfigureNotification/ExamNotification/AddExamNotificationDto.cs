using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Web.Client.DataAnotationValidators;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Learning.Web.Components.Pages.Admin.ConfigureNotification.ExamNotification;

public class AddExamNotificationDto : AddEditExamNotificationRequestDto
{
    [Required]
    [MaxFileSize(1_000_000)]
    public IBrowserFile? ImageFile { get; set; }

    [MaxFileSize(5_000_000)]
    public IBrowserFile? PdfFile { get; set; }

    [MaxFileSize(10_000_000)]
    public IBrowserFile? Video { get; set; }
}