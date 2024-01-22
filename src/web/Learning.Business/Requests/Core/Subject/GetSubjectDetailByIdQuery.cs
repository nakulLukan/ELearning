using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class GetSubjectDetailByIdQuery : IRequest<ApiResponseDto<SubjectDetailDto>>
{
    public int SubjectId { get; set; }
}

public class GetSubjectDetailByIdQueryHandler : IRequestHandler<GetSubjectDetailByIdQuery, ApiResponseDto<SubjectDetailDto>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public GetSubjectDetailByIdQueryHandler(IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<ApiResponseDto<SubjectDetailDto>> Handle(GetSubjectDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var subject = await _dbContext.Subjects
            .Where(x => x.Id == request.SubjectId)
            .Select(x => new SubjectDetailDto
            {
                SubjectId = x.Id,
                ClassId = x.ClassId,
                CourseId = x.Class.CourseId,
                SubjectName = x.Name,
                ClassCode = x.Class.Code,
                ClassName = x.Class.Name,
                CourseName = x.Class.Course.Name,
                CourseCode = x.Class.Course.Code,
                SubjectCode = x.Code,
                SubjectDescription = x.Description,
                ThumbnailImage = x.ThumbnailRelativePath
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Invalid subject id", true);
        subject.ThumbnailImage = GetFilePresignedUrl(subject.ThumbnailImage);
        return new(subject);
    }

    private string GetFilePresignedUrl(string thumbnailImage)
    {
        if (string.IsNullOrEmpty(thumbnailImage))
        {
            return thumbnailImage;
        }

        return _fileStorage.GetPresignedUrl(thumbnailImage);
    }
}

