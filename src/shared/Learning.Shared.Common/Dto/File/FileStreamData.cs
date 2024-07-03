namespace Learning.Shared.Common.Dto.File;

public class FileStreamData
{
    public Stream Stream { get; init; }
    public string FileName { get; init; }
    public long Length { get; init; }

    public FileStreamData(Stream stream, string fileName, long length)
    {
        Stream = stream;
        FileName = fileName;
        Length = length;
    }
}
