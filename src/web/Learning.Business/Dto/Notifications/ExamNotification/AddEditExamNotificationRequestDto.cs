﻿using Learning.Shared.Constants;
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
    public string? Description { get; set; }

    [MaxLength(1000)]
    public string? ImportantPoints { get; set; }

    [Required]
    [MaxLength(250)]
    [Url(ErrorMessage = RegexExpConst.GeneralInValidUrlMessage)]
    public string? GovtLink { get; set; }

    public bool DisplayInHomePage { get; set; }
    public DateTime? ValidTill { get; set; }
}
