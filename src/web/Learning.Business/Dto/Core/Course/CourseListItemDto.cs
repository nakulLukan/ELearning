namespace Learning.Business.Dto.Core.Course;

public class CourseListItemDto
{
    public string CourseName { get; set; }
    public string ShortCode { get; set; }
    public bool IsEnabled { get; set; }
    public string Description { get; set; }
    public string CreatedOn { get; set; }
}
