using Learning.Business.Dto.Core.Course;
using Learning.Business.Impl.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class ClassSelectQuery : IRequest<List<ClassSelectItemDto>>
{
    public int? CourseId { get; set; }
}

public class ClassSelectQueryHandler : IRequestHandler<ClassSelectQuery, List<ClassSelectItemDto>>
{
    readonly IAppDbContext _dbContext;

    public ClassSelectQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ClassSelectItemDto>> Handle(ClassSelectQuery request, CancellationToken cancellationToken)
    {
        var classes = await _dbContext.Classes
            .Where(x => x.IsActive && (!request.CourseId.HasValue || x.CourseId == request.CourseId))
            .OrderBy(x => x.Code)
            .Select(x => new ClassSelectItemDto
            {
                ClassCode = x.Code,
                ClassId = x.Id,
                ClassName = x.Name
            })
            .ToListAsync(cancellationToken);

        return new(classes);
    }
}

