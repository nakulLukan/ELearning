using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.Lesson;

public class AddNewLessonDto
{
    [Required]
    [MaxLength(100)]
    [RegularExpression(RegexExpConst.LessonName, ErrorMessage = RegexExpConst.LessonNameMessage)]
    public string LessonName { get; set; }

    [MaxLength(30)]
    [RegularExpression(RegexExpConst.LessonCode, ErrorMessage = RegexExpConst.LessonCodeMessage)]
    public string LessonCode { get; set; }

    public int ChapterId { get; set; }
    public int VideoId { get; set; }
    public int OrderWrtChapter { get; set; }

    /// <summary>
    /// Flag to indicate whether this lesson can be watched without subscription.
    /// </summary>
    public bool IsPreviewable { get; set; }
}
