using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Dto;
using Learning.Shared.Contracts.HttpContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class DeleteExamNotificationCommand : IRequest<ResponseDto<bool>>
{
    public int ExamNotificationId { get; set; }
}

public class DeleteExamNotificationCommandHandler : IRequestHandler<DeleteExamNotificationCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IAppCache _appCache;
    private readonly IFileStorage _fileStorage;

    public DeleteExamNotificationCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IAppCache appCache,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _appCache = appCache;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<bool>> Handle(DeleteExamNotificationCommand request, CancellationToken cancellationToken)
    {
        var examNotification = await _dbContext.ExamNotifications.AsTracking()
            .Include(x => x.PdfFile)
            .FirstAsync(x => x.Id == request.ExamNotificationId);

        if (examNotification.PdfFile != null)
        {
            await _fileStorage.DeleteFileAsync(examNotification.PdfFile.RelativePath);
        }

        await _fileStorage.DeleteFileAsync(examNotification.ImageRelativePath);
        _dbContext.ExamNotifications.Remove(examNotification);
        await _dbContext.SaveAsync(cancellationToken);
        _appCache.DeleteKey(ExamNotificationCacheKey.ActiveNotificationsKey);
        _appCache.DeleteKey(ExamNotificationCacheKey.ActiveNotificationsDetailKey);
        return new(true);
    }
}

