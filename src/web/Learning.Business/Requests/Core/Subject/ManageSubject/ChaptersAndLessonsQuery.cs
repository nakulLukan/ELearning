using Learning.Business.Dto.Core.Lesson;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Learning.Shared.Common.Extensions;

namespace Learning.Business.Requests.Core.Subject.ManageSubject;

public class ChaptersAndLessonsQuery : IRequest<List<ChapterListViewItemDto>>
{
    public int SubjectId { get; set; }
}

public class ChaptersAndLessonsQueryHandler : IRequestHandler<ChaptersAndLessonsQuery, List<ChapterListViewItemDto>>
{
    readonly IAppDbContext _dbContext;

    public ChaptersAndLessonsQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<List<ChapterListViewItemDto>> Handle(ChaptersAndLessonsQuery request, CancellationToken cancellationToken)
    {
        var subjectExists = await _dbContext.Subjects.AnyAsync(x => x.Id == request.SubjectId, cancellationToken);
        if (!subjectExists)
        {
            throw new AppException("Subject not found", true);
        }
        var chapters = await _dbContext.Chapters.Where(x => x.SubjectId == request.SubjectId)
            .OrderBy(x => x.Order)
            .Select(x => new ChapterListViewItemDto
            {
                ChapterName = x.Name,
                SerialNumber = x.Order,
                ChapterId = x.Id,
                Lessons = x.Lessons.Select(y => new LessonListViewItemDto
                {
                    Order = y.Order,
                    Duration = y.Video.Duration.ToDurationString(),
                    IsActive = y.IsActive,
                    LessionId = y.Id,
                    LessonName = y.Name,
                    IsPreviewable = y.IsPreviewable,
                }).ToList()
            })
            .ToListAsync();
        return chapters;

    }
}
