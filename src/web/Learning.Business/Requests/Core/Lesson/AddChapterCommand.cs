using Learning.Business.Dto.Core.Lesson;
using Learning.Business.Impl.Data;
using Learning.Domain.Core;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Lesson;

public class AddChapterCommand : AddChapterDto, IRequest<ApiResponseDto<int>>
{
}

public class AddChapterCommandHandler : IRequestHandler<AddChapterCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public AddChapterCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ApiResponseDto<int>> Handle(AddChapterCommand request, CancellationToken cancellationToken)
    {
        var chaptersInSubject = await _dbContext.Chapters.Where(x => x.SubjectId == request.SubjectId)
            .Select(x => new Chapter
            {
                Id = x.Id,
                Name = x.Name,
                Order = x.Order
            })
            .ToListAsync(cancellationToken);

        if (chaptersInSubject.Any(x => x.Name == request.ChapterName))
        {
            throw new AppException("A chapter with same name exists.");
        }
        var currTime = AppDateTime.UtcNow;
        var newChapter = new Chapter
        {
            Name = request.ChapterName,
            CreatedOn = currTime,
            LastUpdatedOn = currTime,
            IsActive = true,
            Order = await GetChapterOrder(request, chaptersInSubject),
            SubjectId = request.SubjectId,
        };

        _dbContext.Chapters.Add(newChapter);
        await _dbContext.SaveAsync(cancellationToken);
        return new(newChapter.Id);
    }

    private async Task<int> GetChapterOrder(AddChapterCommand request, List<Chapter> existingChapters)
    {
        if (request.PreviousChapterId == null)
        {
            return (existingChapters.Any() ? existingChapters.Max(x => x.Order) + 1 : 1);
        }

        var prevChapterOrder = existingChapters.First(x => x.Id == request.PreviousChapterId.Value).Order;
        var updatedCount = await _dbContext.Chapters
            .Where(x => x.SubjectId == request.SubjectId && x.Order > prevChapterOrder)
            .ExecuteUpdateAsync(setters => setters.SetProperty(y => y.Order, y => y.Order + 1));
        return prevChapterOrder + 1;
    }
}

