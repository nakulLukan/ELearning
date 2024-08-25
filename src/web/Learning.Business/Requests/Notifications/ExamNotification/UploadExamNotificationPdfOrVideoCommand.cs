using Learning.Business.Impl.Data;
using Learning.Domain.Master;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Dto.File;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Contracts.HttpContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class UploadExamNotificationPdfOrVideoCommand : IRequest<ResponseDto<long>>
{
    public required int ExamNotificationId { get; set; }
    public required FileStreamData File { get; set; }
    public bool IsPdf { get; set; }
}

public class UploadExamNotificationPdfOrVideoCommandHandler : IRequestHandler<UploadExamNotificationPdfOrVideoCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IFileStorage _fileStorage;

    public UploadExamNotificationPdfOrVideoCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<long>> Handle(UploadExamNotificationPdfOrVideoCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var attachmentId = await UploadPdfOrVideoStorage(request, userId, cancellationToken);

        var query = _dbContext.ExamNotifications.Where(x => x.Id == request.ExamNotificationId).AsQueryable();

        if (request.IsPdf)
        {
            await query
                .ExecuteUpdateAsync((setters) => setters.SetProperty(x => x.PdfFileId, attachmentId));
        }
        else
        {
            await query
                .ExecuteUpdateAsync((setters) => setters.SetProperty(x => x.VideoId, attachmentId));
        }

        return new(attachmentId);
    }

    private async Task<long> UploadPdfOrVideoStorage(UploadExamNotificationPdfOrVideoCommand request, string userId, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await request.File.Stream.CopyToAsync(ms);
        var data = await _fileStorage.UploadFileToPublic(ms.ToArray(), request.File.FileName, StoragePathConstant.PublicExamNotificationBasePath, cancellationToken);
        var attachmentRecord = new Attachment()
        {
            FileName = request.File.FileName,
            RelativePath = data.RelativePath,
            Size = request.File.Length,
            CreatedOn = AppDateTime.UtcNow,
            LastUpdatedOn = AppDateTime.UtcNow,
            CreatedBy = userId,
            LastUpdatedBy = userId,
        };

        _dbContext.Attachments.Add(attachmentRecord);
        await _dbContext.SaveAsync(cancellationToken);
        return attachmentRecord.Id;
    }
}

