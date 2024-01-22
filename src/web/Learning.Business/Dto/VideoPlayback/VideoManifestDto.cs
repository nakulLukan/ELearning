using Learning.Shared.Common.Enums;

namespace Learning.Business.Dto.VideoPlayback;

public class VideoManifestDto
{
    public int VideoId { get; set; }
    public string ManifestUrl { get; set; }
    public VideoPlaybackStatus Status { get; set; }
}
