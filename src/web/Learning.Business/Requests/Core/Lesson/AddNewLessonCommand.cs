using Learning.Business.Dto.Core.Lesson;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Lesson;

public class AddNewLessonCommand : AddNewLessonDto, IRequest<ApiResponseDto<int>>
{
}

public class AddNewLessonCommandHandler : IRequestHandler<AddNewLessonCommand, ApiResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;

    public AddNewLessonCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ApiResponseDto<int>> Handle(AddNewLessonCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _dbContext.Chapters
            .Include(x => x.Lessons)
            .FirstOrDefaultAsync(x => x.Id == request.ChapterId, cancellationToken);
        if (chapter is null)
        {
            throw new Exception();
        }

        if (chapter.Lessons.Any(x => x.Name == request.LessonName))
        {
            throw new AppException("Another with same name exists in this chapter.");
        }

        var nextOrder = request.OrderWrtChapter == 0 && chapter.Lessons.Any()?
            chapter.Lessons.Max(x => x.Order) + 1 :
            request.OrderWrtChapter;

        var currTime = AppDateTime.UtcNow;
        Domain.Core.Lesson newLesson = new()
        {
            IsActive = true,
            ChapterId = request.ChapterId,
            Order = nextOrder,
            Code = request.LessonCode,
            Name = request.LessonName,
            VideoId = request.VideoId,
            IsPreviewable = request.IsPreviewable,
        };

        _dbContext.Lessons.Add(newLesson);
        await _dbContext.SaveAsync(cancellationToken);
        return new(newLesson.Id);
    }
}

