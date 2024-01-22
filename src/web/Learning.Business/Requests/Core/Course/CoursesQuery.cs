using Learning.Business.Dto.Core.Course;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class CoursesQuery : IRequest<PaginatedResponse<CourseListItemDto>>
{
    public string? SearchValue { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class CoursesQueryHandler : IRequestHandler<CoursesQuery, PaginatedResponse<CourseListItemDto>>
{
    readonly IAppDbContext _dbContext;

    public CoursesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedResponse<CourseListItemDto>> Handle(CoursesQuery request, CancellationToken cancellationToken)
    {
        var coursesQuery = _dbContext.Courses
            .Where(x => string.IsNullOrEmpty(request.SearchValue) || (x.Code.Contains(request.SearchValue) || x.Name.Contains(request.SearchValue)))
            .Select(x => new CourseListItemDto
            {
                CourseName = x.Name,
                Description = x.Description,
                IsEnabled = x.IsActive,
                CreatedOn = x.CreatedOn.HasValue ? x.CreatedOn.Value.ToLocalDateTimeString() : string.Empty,
                ShortCode = x.Code,
            }).AsQueryable();
        var count = await coursesQuery.CountAsync(cancellationToken);

        coursesQuery = coursesQuery
            .OrderBy(x => x.ShortCode)
            .AsQueryable();
        if (request.Skip.HasValue)
        {
            coursesQuery = coursesQuery.Skip(request.Skip.Value);
        }
        if (request.Take.HasValue)
        {
            coursesQuery = coursesQuery.Take(request.Take.Value);
        }

        var courses = await coursesQuery.ToListAsync(cancellationToken);
        return new PaginatedResponse<CourseListItemDto>(courses, count);
    }
}

