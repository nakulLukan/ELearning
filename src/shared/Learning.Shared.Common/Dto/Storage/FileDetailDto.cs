namespace Learning.Shared.Common.Dto.Storage;

public class FileDetailDto
{
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime LastModified { get; set; }
}