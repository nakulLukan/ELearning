using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Services.ExamNotification;

public class ExamNotificationManager : IExamNotificationManager
{
    private readonly IAppDbContext _dbContext;
    private readonly IAppCache _appCache;
    private readonly IFileStorage _fileStorage;

    public ExamNotificationManager(IAppDbContextFactory dbContext,
                                   IAppCache appCache,
                                   IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _appCache = appCache;
        _fileStorage = fileStorage;
    }

    public async Task<List<ActiveExamNotificationCacheDto>> GetAllActiveExamNotifications(CancellationToken cancellationToken)
    {
        (bool isNotificationsAvailableInCache, List<ActiveExamNotificationCacheDto>? examNotifications)
            = _appCache.Get<List<ActiveExamNotificationCacheDto>>(ExamNotificationCacheKey.ActiveNotificationsKey);
        if (!isNotificationsAvailableInCache)
        {
            var activeNotifications = await _dbContext.ExamNotifications
                            .Where(x => !x.ValidTill.HasValue || x.ValidTill.Value >= DateOnly.FromDateTime(AppDateTime.UtcNow.DateTime))
                            .Select(x => new ActiveExamNotificationCacheDto
                            {
                                ValidTill = x.ValidTill,
                                Title = x.NotificationTitle,
                                Description = x.Description,
                                NotificationId = x.Id,
                                ImageRelativePath = _fileStorage.GetObjectUrl(x.ImageRelativePath),
                                DisplayInHomepage = x.DisplayInHomePage,
                            })
                            .ToListAsync(cancellationToken);
            examNotifications = _appCache.Set(ExamNotificationCacheKey.ActiveNotificationsKey, activeNotifications);
        }

        return examNotifications;
    }
}
