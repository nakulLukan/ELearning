using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject.ManageSubject;

public class AddSubjectCommand : AddSubjectDto, IRequest<ApiResponseDto<int>>
{
}
public class AddSubjectCommandHandler : IRequestHandler<AddSubjectCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;

    public AddSubjectCommandHandler(IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<ApiResponseDto<int>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Subjects.AnyAsync(x => request.ShortCode == x.Code || x.ClassId == request.ClassId && x.Name == request.SubjectName, cancellationToken);
        if (exists)
        {
            throw new AppException("Another subject with same short code or name already exists.");
        }

        var currTime = AppDateTime.UtcNow;

        var subject = new Domain.Core.Subject
        {
            Name = request.SubjectName,
            Code = request.ShortCode,
            Description = request.Description,
            IsActive = request.IsActive,
            CreatedOn = currTime,
            LastUpdatedOn = currTime,
            ClassId = request.ClassId,
            SubjectGroupLookupId = request.SubjectGroupLookupId,
        };

        _dbContext.Subjects.Add(subject);
        await _dbContext.SaveAsync(cancellationToken);
        await SaveThumbnail(request, subject, cancellationToken);
        return new(subject.Id);
    }

    private async Task SaveThumbnail(AddSubjectCommand request, Domain.Core.Subject subject, CancellationToken cancellationToken)
    {
        var relativePath = StoragePathConstant.SubjectThumbnailBasePath(subject.Id);
        await _fileStorage.UploadFile(request.ThumbnailData, "thumbnail.png", relativePath, cancellationToken);
        subject.ThumbnailRelativePath = relativePath + "/" + "thumbnail.png";
        await _dbContext.SaveAsync(cancellationToken);
    }
}

