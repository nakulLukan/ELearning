using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Notifications.ExamNotification;

public abstract class AddEditExamNotificationRequestDto
{
    [Required]
    [MaxLength(50)]
    [RegularExpression(RegexExpConst.ExamNotificationTitle, ErrorMessage = RegexExpConst.ExamNotificationTitleMessage)]
    public string? Title { get; set; }

    [Required]
    [MaxLength(500)]
    [RegularExpression(RegexExpConst.GeneralDescription, ErrorMessage = RegexExpConst.GeneralDescriptionMessage)]
    public string? Description { get; set; }

    [MaxLength(1000)]
    [RegularExpression(RegexExpConst.ImportantPoints, ErrorMessage = RegexExpConst.ImportantPointsMessage)]
    public string? ImportantPoints { get; set; }

    [Required]
    [MaxLength(250)]
    [Url(ErrorMessage = RegexExpConst.GeneralInValidUrlMessage)]
    public string? GovtLink { get; set; }

    public bool DisplayInHomePage { get; set; }
    public DateTime? ValidTill { get; set; }
}
