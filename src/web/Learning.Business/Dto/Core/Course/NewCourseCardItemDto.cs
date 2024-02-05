namespace Learning.Business.Dto.Core.Course;

public class NewCourseCardItemDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public string SubjectCode { get; set; }
    public string ClassName { get; set; }
    public string CourseName { get; set; }
    public string SubjectDescription { get; set; }
    public float Price { get; set; }
    public float DiscountedPrice { get; set; }
    public string ImgSrc { get; set; }
}
