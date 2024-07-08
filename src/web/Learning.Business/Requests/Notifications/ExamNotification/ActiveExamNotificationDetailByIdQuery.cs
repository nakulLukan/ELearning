using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class ActiveExamNotificationDetailByIdQuery : IRequest<ActiveExamNotificationDetailDto>
{
    public required int ExamNotificationId { get; set; }
}

public class ActiveExamNotificationDetailByIdQueryHandler : IRequestHandler<ActiveExamNotificationDetailByIdQuery, ActiveExamNotificationDetailDto>
{
    private readonly IAppCache _appCache;
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public ActiveExamNotificationDetailByIdQueryHandler(
        IAppCache appCache,
        IAppDbContextFactory dbContext,
        IFileStorage fileStorage)
    {
        _appCache = appCache;
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ActiveExamNotificationDetailDto> Handle(ActiveExamNotificationDetailByIdQuery request, CancellationToken cancellationToken)
    {
        // Check cache if the exam notification is available. If not then fetch from database and update the cache
        (bool isCacheNotEmpty, List<ActiveExamNotificationDetailCacheDto>? examNotifications)
            = _appCache.Get<List<ActiveExamNotificationDetailCacheDto>>(ExamNotificationCacheKey.ActiveNotificationsDetailKey);
        if (!isCacheNotEmpty || (examNotifications == null || examNotifications.Any(x => x.ExamNotificationId != request.ExamNotificationId)))
        {
            examNotifications = await FetchFromDatabase(request.ExamNotificationId, examNotifications, cancellationToken);
        }

        return (examNotifications ?? [])
            .Where(x => x.ExamNotificationId == request.ExamNotificationId)
            .Select(x => new ActiveExamNotificationDetailDto()
            {
                Description = x.Description,
                GovtLink = x.GovtLink,
                ImageAbsPath = x.ImageAbsPath,
                Title = x.Title,
                VideoAbsUrl = x.VideoAbsUrl,
                ExamNotificationId = request.ExamNotificationId,
                ImportantPoints = x.ImportantPoints?.Split('\n', StringSplitOptions.RemoveEmptyEntries),
                PdfFileAbsUrl = x.PdfFileAbsUrl,
                ValidTill = x.ValidTill,
            })
            .First();
    }

    private async Task<List<ActiveExamNotificationDetailCacheDto>> FetchFromDatabase(
        int examNotificationId,
        List<ActiveExamNotificationDetailCacheDto>? examNotifications,
        CancellationToken cancellationToken)
    {
        var activeNotification = await _dbContext.ExamNotifications
            .Where(x => x.Id == examNotificationId)
            .Select(x => new ActiveExamNotificationDetailCacheDto
            {
                GovtLink = x.GovtLink,
                ImageAbsPath = _fileStorage.GetObjectUrl(x.ImageRelativePath),
                ExamNotificationId = x.Id,
                ImportantPoints = x.ImportantPoints,
                ValidTill = x.ValidTill,
                PdfFileAbsUrl = x.PdfFile != null ? _fileStorage.GetObjectUrl(x.PdfFile.RelativePath) : string.Empty,
                VideoAbsUrl = x.Video != null ? _fileStorage.GetObjectUrl(x.Video.RelativePath) : string.Empty,
                Title = x.NotificationTitle,
                Description = x.Description,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Could not find the requested exam notification details.");
        if (examNotifications == null)
        {
            examNotifications = new();
        }
        examNotifications.Add(activeNotification);
        examNotifications = _appCache.Set(ExamNotificationCacheKey.ActiveNotificationsDetailKey, examNotifications);
        return examNotifications;
    }
}

