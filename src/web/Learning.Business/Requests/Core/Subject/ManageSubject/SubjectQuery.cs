using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject.ManageSubject;

public class SubjectQuery : IRequest<PaginatedResponse<SubjectListItemDto>>
{
    public string? SearchValue { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class SubjectQueryHandler : IRequestHandler<SubjectQuery, PaginatedResponse<SubjectListItemDto>>
{
    readonly IAppDbContext _dbContext;

    public SubjectQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<SubjectListItemDto>> Handle(SubjectQuery request, CancellationToken cancellationToken)
    {
        var subjectsQuery = _dbContext.Subjects
            .Where(x => string.IsNullOrEmpty(request.SearchValue) || x.Code.Contains(request.SearchValue) || x.Name.Contains(request.SearchValue))
            .AsQueryable();

        var count = await subjectsQuery.CountAsync(cancellationToken);

        subjectsQuery = subjectsQuery
            .OrderBy(x => x.Code)
                .ThenBy(x => x.Class.Code)
                    .ThenBy(x => x.Class.Course.Code)
            .AsQueryable();
        if (request.Skip.HasValue)
        {
            subjectsQuery = subjectsQuery.Skip(request.Skip.Value);
        }
        if (request.Take.HasValue)
        {
            subjectsQuery = subjectsQuery.Take(request.Take.Value);
        }
        var subjects = await subjectsQuery.Select(x => new SubjectListItemDto
        {
            SubjectId = x.Id,
            SubjectName = x.Name,
            ShortCode = x.Code,
            ClassCode = x.Class.Code,
            Description = x.Description,
            IsEnabled = x.IsActive,
            ClassName = x.Class.Name,
            CourseName = x.Class.Course.Name,
            CourseCode = x.Class.Course.Code,
            CreatedOn = x.CreatedOn.HasValue ? x.CreatedOn.Value.ToLocalDateTimeString() : string.Empty,
        }).ToListAsync(cancellationToken);
        return new(subjects, count);
    }
}

