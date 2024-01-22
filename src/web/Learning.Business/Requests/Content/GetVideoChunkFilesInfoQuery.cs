using Learning.Business.Dto.Content;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Content;

public class GetVideoChunkFilesInfoQuery : IRequest<List<VideoChunkFileListItemDto>>
{
    public int VideoId { get; set; }
}

public class GetVideoChunkFilesInfoQueryHandler : IRequestHandler<GetVideoChunkFilesInfoQuery, List<VideoChunkFileListItemDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _storage;

    public GetVideoChunkFilesInfoQueryHandler(IAppDbContext dbContext, IFileStorage storage)
    {
        _dbContext = dbContext;
        _storage = storage;
    }

    public async Task<List<VideoChunkFileListItemDto>> Handle(GetVideoChunkFilesInfoQuery request, CancellationToken cancellationToken)
    {
        var videoDetails = await _dbContext.Videos.FirstOrDefaultAsync(x => x.Id == request.VideoId, cancellationToken)
            ?? throw new AppException("Video not found", true);

        var files = await _storage.ListFilesAsync(videoDetails.RelativeUrl);

        return files.Select(x => new VideoChunkFileListItemDto
        {
            FileSize = x.FileSize.ToFileSizeString(),
            FileName = Path.GetFileName(x.FileName),
            LastModified = x.LastModified.ToLocalDateTimeString(),
        }).ToList();
    }
}
