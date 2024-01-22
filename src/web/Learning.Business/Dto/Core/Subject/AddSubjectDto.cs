using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.Subject;

public class AddSubjectDto
{
    [Required]
    [RegularExpression(RegexExpConst.SubjectCode, ErrorMessage = RegexExpConst.SubjectCodeMessage)]
    [MaxLength(20)]
    public string ShortCode { get; set; }

    [Required]
    [RegularExpression(RegexExpConst.SubjectName, ErrorMessage = RegexExpConst.SubjectNameMessage)]
    [MaxLength(30)]
    public string SubjectName { get; set; }

    [MaxLength(500)]
    [RegularExpression(RegexExpConst.GeneralDescription, ErrorMessage = RegexExpConst.GeneralDescriptionMessage)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int? ClassId { get; set; }

    [Required]
    public int? CourseId { get; set; }

    public int? SubjectGroupLookupId { get; set; }

    public byte[] ThumbnailData { get; set; }
}
