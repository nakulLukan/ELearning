using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Core.Course;

public class AddCourseDto
{
    [Required]
    [RegularExpression(RegexExpConst.CourseCode, ErrorMessage = RegexExpConst.CourseCodeMessage)]
    [MaxLength(10)]
    public string ShortCode { get; set; }

    [Required]
    [RegularExpression(RegexExpConst.CourseName, ErrorMessage = RegexExpConst.CourseNameMessage)]
    [MaxLength(30)]
    public string CourseName { get; set; }

    [MaxLength(500)]
    [RegularExpression(RegexExpConst.GeneralDescription, ErrorMessage = RegexExpConst.GeneralDescriptionMessage)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;
}
