using Learning.Business.Dto.VideoPlayback;
using Learning.Business.Impl.Data;
using Learning.Business.Services.Core;
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
    readonly SubjectManager _subjectManager;

    public VideoManifestQueryHandler(IAppDbContext dbContext, IFileStorage fileStorage, SubjectManager subjectManager)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _subjectManager = subjectManager;
    }

    public async Task<ApiResponseDto<VideoManifestDto>> Handle(VideoManifestQuery request, CancellationToken cancellationToken)
    {
        // Get subject and check if the user has subscribed the associated subject
        var lessonDetails = await _dbContext.Lessons.Where(x => x.VideoId == request.VideoId)
            .Select(x => new
            {
                x.Chapter.SubjectId,
                x.IsPreviewable
            }).FirstOrDefaultAsync(cancellationToken);
        if (lessonDetails == null)
        {
            return new ApiResponseDto<VideoManifestDto>(new()
            {
                Status = Shared.Common.Enums.VideoPlaybackStatus.VideoNotFound
            });
        }
        var isSubscribed = await _subjectManager.IsSubjectSubscribed(lessonDetails.SubjectId, cancellationToken);
        
        if (!isSubscribed && !lessonDetails.IsPreviewable)
        {
            return new(new()
            {
                Status = Shared.Common.Enums.VideoPlaybackStatus.NotSubscribed
            });
        }
        var manifestDetails = await _dbContext.Videos
            .Where(x => x.Id == request.VideoId)
            .Select(x => new
            {
                x.RelativeUrl,
                x.MpdFileName
            })
            .FirstAsync(cancellationToken);

        var url = _fileStorage.GetObjectUrl(manifestDetails.RelativeUrl, manifestDetails.MpdFileName);
        return new ApiResponseDto<VideoManifestDto>(new()
        {
            ManifestUrl = url,
            Status = Shared.Common.Enums.VideoPlaybackStatus.Success,
            VideoId = request.VideoId,
        });
    }
}

