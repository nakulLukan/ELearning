using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class GetSubjectIdByCodeQuery : IRequest<ApiResponseDto<int>>
{
    /// <summary>
    /// Subject short code
    /// </summary>
    public string SubjectCode { get; set; }
}

public class GetSubjectIdByCodeQueryHandler : IRequestHandler<GetSubjectIdByCodeQuery, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public GetSubjectIdByCodeQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ApiResponseDto<int>> Handle(GetSubjectIdByCodeQuery request, CancellationToken cancellationToken)
    {
        var subjectId = await _dbContext.Subjects
            .Where(x => x.Code == request.SubjectCode)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("This course cannot be found.", true);
        return new(subjectId);
    }
}

