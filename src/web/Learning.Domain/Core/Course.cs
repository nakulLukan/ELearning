namespace Learning.Domain.Core;

public class Course : DomainBase
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public List<ClassDivision> Classes { get; set; }
}
