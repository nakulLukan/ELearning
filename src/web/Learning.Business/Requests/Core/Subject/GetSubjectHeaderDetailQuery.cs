using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class GetSubjectHeaderDetailQuery : IRequest<SubjectHeaderDetailDto>
{
    public int SubjectId { get; set; }
}

public class GetSubjectHeaderDetailQueryHandler : IRequestHandler<GetSubjectHeaderDetailQuery, SubjectHeaderDetailDto>
{
    readonly IAppDbContext _dbContext;

    public GetSubjectHeaderDetailQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<SubjectHeaderDetailDto> Handle(GetSubjectHeaderDetailQuery request, CancellationToken cancellationToken)
    {
        var headerDetails = await _dbContext.Subjects
            .Where(x => x.Id == request.SubjectId)
            .Select(x => new SubjectHeaderDetailDto
            {
                SubjectName = x.Name,
                ClassName = x.Class.Name,
                CourseName = x.Class.Course.Name,
                ClassId = x.ClassId.Value,
                CourseId = x.Class.CourseId.Value,
                SubjectId = x.Id
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Course not found");
        return headerDetails;
    }
}

