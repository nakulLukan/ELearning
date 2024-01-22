namespace Learning.Business.Dto.Core.Subject;

public class SubjectDetailDto
{
    public int? SubjectId { get; set; }
    public int? ClassId { get; set; }
    public int? CourseId { get; set; }

    public string SubjectName { get; set; }
    public string ClassName { get; set; }
    public string CourseName { get; set; }

    public string SubjectCode { get; set; }
    public string ClassCode { get; set; }
    public string CourseCode { get; set; }

    public string SubjectDescription { get; set; }

    /// <summary>
    /// Thumbnail pre-signed url
    /// </summary>
    public string ThumbnailImage { get; set; }
}
