using Learning.Business.Dto.VideoPlayback;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.VideoPlayback;

public class VideoManifestQuery : IRequest<ApiResponseDto<VideoManifestDto>>
{
    public int VideoId { get; set; }
}
public class VideoManifestQueryHandler : IRequestHandler<VideoManifestQuery, ApiResponseDto<VideoManifestDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public VideoManifestQueryHandler(IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<ApiResponseDto<VideoManifestDto>> Handle(VideoManifestQuery request, CancellationToken cancellationToken)
    {
        var manifestDetails = await _dbContext.Videos.FirstOrDefaultAsync(x => x.Id == request.VideoId, cancellationToken);
        if (manifestDetails == null)
        {
            return new ApiResponseDto<VideoManifestDto>(new()
            {
                Status = Shared.Common.Enums.VideoPlaybackStatus.VideoNotFound
            });
        }

        var url = _fileStorage.GetObjectUrl(manifestDetails.RelativeUrl, manifestDetails.MpdFileName);
        return new ApiResponseDto<VideoManifestDto>(new()
        {
            ManifestUrl = url,
            Status = Shared.Common.Enums.VideoPlaybackStatus.Success,
            VideoId = request.VideoId
        });
    }
}

