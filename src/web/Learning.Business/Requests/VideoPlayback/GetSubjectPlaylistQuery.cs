using Learning.Business.Contracts.HttpContext;
using Learning.Business.Dto.VideoPlayback;
using Learning.Business.Impl.Data;
using Learning.Business.Services.Core;
using Learning.Shared.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.VideoPlayback;

public class GetSubjectPlaylistQuery : IRequest<SubjectPlaylistDto>
{
    public int SubjectId { get; set; }
}

public class GetSubjectPlaylistQueryHandler : IRequestHandler<GetSubjectPlaylistQuery, SubjectPlaylistDto>
{
    readonly IAppDbContext _dbContext;
    readonly IRequestContext _requestContext;

    public GetSubjectPlaylistQueryHandler(IAppDbContext dbContext, IRequestContext requestContext)
    {
        _dbContext = dbContext;
        _requestContext = requestContext;
    }

    public async Task<SubjectPlaylistDto> Handle(GetSubjectPlaylistQuery request, CancellationToken cancellationToken)
    {
        var authDetail = await _requestContext.IsLoggedIn();
        var chapters = await _dbContext.Chapters
            .Where(x => x.SubjectId == request.SubjectId)
            .Select(x => new ContentChapterListItemDto
            {
                ChapterId = x.Id,
                ChapterName = x.Name,
                Lessons = x.Lessons.Select(y => new ContentLessonListItemDto
                {
                    LessonId = y.Id,
                    Duration = y.Video.Duration.ToDurationString(),
                    HasCompleted = false,
                    IsLocked = LessonManager.IsLessonLocked(y.IsPreviewable, authDetail.IsAuthenticated, authDetail.IsAdmin),
                    LessonName = y.Name,
                }).ToList(),
            })
            .ToListAsync(cancellationToken);

        return new()
        {
            Chapters = chapters
        };
    }
}

