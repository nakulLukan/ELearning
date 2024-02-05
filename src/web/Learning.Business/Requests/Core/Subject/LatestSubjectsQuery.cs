using Learning.Business.Dto.Core.Course;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class LatestSubjectsQuery : IRequest<List<NewCourseCardItemDto>>
{
}

public class NewSubjectsQueryHandler : IRequestHandler<LatestSubjectsQuery, List<NewCourseCardItemDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public NewSubjectsQueryHandler(IAppDbContextFactory dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<List<NewCourseCardItemDto>> Handle(LatestSubjectsQuery request, CancellationToken cancellationToken)
    {
        var newCourses = await _dbContext.Subjects
            .OrderByDescending(x => x.CreatedOn)
            .Take(10)
            .Select(x => new NewCourseCardItemDto
            {
                ClassName = x.Class.Name,
                CourseName = x.Class.Course.Name,
                Price = x.SubscriptionDetail.OriginalPrice,
                DiscountedPrice = x.SubscriptionDetail.DiscountedPrice,
                SubjectDescription = x.Description,
                SubjectId = x.Id,
                ImgSrc = x.ThumbnailRelativePath,
                SubjectName = x.Name,
                SubjectCode = x.Code
            })
            .ToListAsync(cancellationToken);

        foreach (var newCourse in newCourses)
        {
            newCourse.ImgSrc = _fileStorage.GetPresignedUrl(newCourse.ImgSrc);
        }
        return newCourses;
    }
}

