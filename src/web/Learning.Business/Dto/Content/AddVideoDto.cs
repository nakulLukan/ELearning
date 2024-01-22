namespace Learning.Business.Dto.Content;

public class AddVideoDto
{
    public int Id { get; set; }
    public string VideoFolderRelativeUrl { get; set; }
    public string MpdFileName { get; set; }
    public long FileSize { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Length of the video in seconds
    /// </summary>
    public int Duration { get; set; }
}
