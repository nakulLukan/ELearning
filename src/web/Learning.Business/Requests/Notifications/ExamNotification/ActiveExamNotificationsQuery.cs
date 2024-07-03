using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class ActiveExamNotificationsQuery : IRequest<ActiveExamNotificationsQueryResponseDto[]>
{
}

public class ActiveExamNotificationsQueryHandler : IRequestHandler<ActiveExamNotificationsQuery, ActiveExamNotificationsQueryResponseDto[]>
{
    private readonly IAppCache _appCache;
    private readonly IAppDbContext _dbContext;

    public ActiveExamNotificationsQueryHandler(IAppCache appCache, IAppDbContextFactory dbContext)
    {
        _appCache = appCache;
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ActiveExamNotificationsQueryResponseDto[]> Handle(ActiveExamNotificationsQuery request, CancellationToken cancellationToken)
    {
        (bool isNotificationsAvailableInCache, List<ActiveExamNotificationCacheDto>? examNotifications)
            = _appCache.Get<List<ActiveExamNotificationCacheDto>>(ExamNotificationCacheKey.ActiveNotificationsKey);
        if (!isNotificationsAvailableInCache)
        {
            examNotifications = await FetchFromDatabase(examNotifications, cancellationToken);
        }

        return (examNotifications ?? [])
            .Select(x => new ActiveExamNotificationsQueryResponseDto
            {
                Title = x.Title,
                Description = x.Description,
                ImagePath = x.ImageRelativePath,
                NotificationId = x.NotificationId
            })
            .ToArray();
    }

    private async Task<List<ActiveExamNotificationCacheDto>?> FetchFromDatabase(List<ActiveExamNotificationCacheDto>? examNotifications, CancellationToken cancellationToken)
    {
        var activeNotifications = await _dbContext.ExamNotifications
                        .Where(x => x.DisplayInHomePage)
                        .Where(x => !x.ValidTill.HasValue || x.ValidTill.Value <= DateOnly.FromDateTime(AppDateTime.UtcNow.DateTime))
                        .Select(x => new ActiveExamNotificationCacheDto
                        {
                            Title = x.NotificationTitle,
                            Description = x.Description,
                            NotificationId = x.Id,
                            ImageRelativePath = x.ImageRelativePath
                        })
                        .ToListAsync(cancellationToken);

        examNotifications = _appCache.Set(ExamNotificationCacheKey.ActiveNotificationsKey, activeNotifications);
        return examNotifications;
    }
}

