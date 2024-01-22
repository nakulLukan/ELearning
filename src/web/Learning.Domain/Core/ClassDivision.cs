namespace Learning.Domain.Core;

public class ClassDivision : DomainBase
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? CourseId { get; set; }

    public Course Course { get; set; }
    public List<Subject> Subjects { get; set; }
}
