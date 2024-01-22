using Learning.Business.Dto.Content;
using Learning.Business.Impl.Data;
using Learning.Domain.Content;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;

namespace Learning.Business.Requests.Content;

public class AddVideoCommand : AddVideoDto, IRequest<ApiResponseDto<int>>
{
}

public class AddVideoCommandHandler : IRequestHandler<AddVideoCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public AddVideoCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponseDto<int>> Handle(AddVideoCommand request, CancellationToken cancellationToken)
    {
        var currTime = AppDateTime.UtcNow;
        var video = new Video
        {
            Code = null,
            FileSize = request.FileSize,
            Duration = request.Duration,
            MpdFileName = request.MpdFileName,
            RelativeUrl = request.VideoFolderRelativeUrl,
            Name = request.Name,
            LastUpdatedOn = currTime,
            CreatedOn = currTime,
        };

        _dbContext.Videos.Add(video);
        await _dbContext.SaveAsync(cancellationToken);
        return new(video.Id);
    }
}
