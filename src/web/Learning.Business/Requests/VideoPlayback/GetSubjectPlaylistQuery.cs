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
    readonly SubjectManager _subjectManager;

    public GetSubjectPlaylistQueryHandler(IAppDbContext dbContext, SubjectManager subjectManager, IRequestContext requestContext)
    {
        _dbContext = dbContext;
        _subjectManager = subjectManager;
        _requestContext = requestContext;
    }

    public async Task<SubjectPlaylistDto> Handle(GetSubjectPlaylistQuery request, CancellationToken cancellationToken)
    {
        // Turn this flag if user has purchased the subject or logged in user is admin.
        var isSubscribed = await _subjectManager.IsSubjectSubscribed(request.SubjectId, cancellationToken)
            || await _requestContext.IsAdmin();

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
                    IsLocked = LessonManager.IsLessonLocked(y.IsPreviewable, isSubscribed),
                    LessonName = y.Name,
                }).ToList(),
            })
            .ToListAsync(cancellationToken);

        var subjectCode = await _dbContext.Subjects.Where(x=>x.Id == request.SubjectId)
            .Select(x=>x.Code)
            .FirstAsync(cancellationToken);
        return new()
        {
            Chapters = chapters,
            IsSubscribed = isSubscribed,
            SubjectCode = subjectCode
        };
    }
}

