namespace Learning.Domain.Core;

public class Chapter : DomainBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int SubjectId { get; set; }
    public int Order { get; set; }

    public List<Lesson> Lessons { get; set; }
    public Subject Subject { get; set; }
}
