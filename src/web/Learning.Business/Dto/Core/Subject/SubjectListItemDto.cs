namespace Learning.Business.Dto.Core.Subject;

public class SubjectListItemDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string ShortCode { get; set; }
    public string ClassName { get; set; }
    public string ClassCode { get; set; }
    public bool IsEnabled { get; set; }
    public string Description { get; set; }
    public string CreatedOn { get; set; }
    public string CourseName { get; set; }
    public string CourseCode { get; set; }
}
