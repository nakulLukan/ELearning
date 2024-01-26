using Learning.Business.Dto.Core.ClassDivision;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class ClassDivisionsQuery : IRequest<PaginatedResponse<ClassDivisionListItemDto>>
{
    public string? SearchValue { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class ClassDivisionsQueryHandler : IRequestHandler<ClassDivisionsQuery, PaginatedResponse<ClassDivisionListItemDto>>
{
    readonly IAppDbContext _dbContext;

    public ClassDivisionsQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<ClassDivisionListItemDto>> Handle(ClassDivisionsQuery request, CancellationToken cancellationToken)
    {
        var classesQuery = _dbContext.Classes
            .Where(x => string.IsNullOrEmpty(request.SearchValue) || (x.Code.Contains(request.SearchValue) || x.Name.Contains(request.SearchValue)))
            .AsQueryable();
            
        var count = await classesQuery.CountAsync(cancellationToken);

        var classesListQuery = classesQuery
            .Select(x => new ClassDivisionListItemDto
            {
                CourseName = x.Course.Name,
                CourseCode = x.Course.Code,
                Description = x.Description,
                IsEnabled = x.IsActive,
                CreatedOn = x.CreatedOn.HasValue ? x.CreatedOn.Value.ToLocalDateTimeString() : string.Empty,
                ShortCode = x.Code,
                ClassName = x.Name,
            }).AsQueryable()
            .OrderBy(x => x.ShortCode)
                .ThenBy(x => x.CourseCode)
            .AsQueryable();
        if (request.Skip.HasValue)
        {
            classesListQuery = classesListQuery.Skip(request.Skip.Value);
        }
        if (request.Take.HasValue)
        {
            classesListQuery = classesListQuery.Take(request.Take.Value);
        }

        var classes = await classesListQuery.ToListAsync(cancellationToken);
        return new(classes, count);
    }
}

