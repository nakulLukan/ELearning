namespace Learning.Domain.Master;

public class Attachment : DomainBase
{
    public long Id { get; set; }
    public required string FileName { get; set; }
    public required string RelativePath { get; set; }
    public long Size { get; set; }
}
