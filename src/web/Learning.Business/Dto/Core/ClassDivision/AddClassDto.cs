using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.ClassDivision;

public class AddClassDto
{
    [Required]
    [RegularExpression(RegexExpConst.ClassCode, ErrorMessage = RegexExpConst.ClassCodeMessage)]
    [MaxLength(15)]
    public string ShortCode { get; set; }

    [Required]
    [RegularExpression(RegexExpConst.ClassName, ErrorMessage = RegexExpConst.ClassNameMessage)]
    [MaxLength(30)]
    public string ClassName { get; set; }

    [MaxLength(500)]
    [RegularExpression(RegexExpConst.GeneralDescription, ErrorMessage = RegexExpConst.GeneralDescriptionMessage)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int? CourseId { get; set; }
}
