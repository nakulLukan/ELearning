using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.Lesson;

public class AddChapterDto
{
    [Required]
    [RegularExpression(RegexExpConst.ChapterName, ErrorMessage = RegexExpConst.ChapterNameMessage)]
    [MaxLength(50)]
    public string ChapterName { get; set; }

    public int SubjectId { get; set; }

    /// <summary>
    /// This value can be used to identify the previous chapter and get the order for this chapter.
    /// </summary>
    public int? PreviousChapterId { get; set; }
}
