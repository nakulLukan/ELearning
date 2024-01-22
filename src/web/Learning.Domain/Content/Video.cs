namespace Learning.Domain.Content;

public class Video : DomainBase
{
    public int Id { get; set; }
    public string? Code { get; set; }

    /// <summary>
    /// Video file name given by the user
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Relative url to the file without file name.
    /// </summary>
    public string RelativeUrl { get; set; }

    /// <summary>
    /// Only the filename with extension
    /// </summary>
    public string MpdFileName { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Length of the video in seconds
    /// </summary>
    public int Duration { get; set; }
}
