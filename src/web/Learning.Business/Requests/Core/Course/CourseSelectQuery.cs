using Learning.Business.Dto.Core.Course;
using Learning.Business.Impl.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class CourseSelectQuery : IRequest<List<CourseSelectItemDto>>
{
}

public class CourseSelectQueryHandler : IRequestHandler<CourseSelectQuery, List<CourseSelectItemDto>>
{
    readonly IAppDbContext _dbContext;

    public CourseSelectQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CourseSelectItemDto>> Handle(CourseSelectQuery request, CancellationToken cancellationToken)
    {
        var courses = await _dbContext.Courses
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .Select(x => new CourseSelectItemDto
            {
                CourseCode = x.Code,
                CourseId = x.Id,
                CourseName = x.Name,
            })
            .ToListAsync();
        
        return new(courses);
    }
}

