using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification;
using MediatR;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class ActiveExamNotificationsQuery : IRequest<ExamNotificationCardItemDto[]>
{
}

public class ActiveExamNotificationsQueryHandler : IRequestHandler<ActiveExamNotificationsQuery, ExamNotificationCardItemDto[]>
{
    private readonly IExamNotificationManager _examNotificationManager;

    public ActiveExamNotificationsQueryHandler(
        IExamNotificationManager examNotificationManager)
    {
        _examNotificationManager = examNotificationManager;
    }

    public async Task<ExamNotificationCardItemDto[]> Handle(ActiveExamNotificationsQuery request, CancellationToken cancellationToken)
    {
        var examNotifications = await _examNotificationManager.GetAllActiveExamNotifications(cancellationToken);
        return (examNotifications ?? [])
            .OrderByDescending(x => x.ValidTill)
            .Select(x => new ExamNotificationCardItemDto
            {
                Title = x.Title,
                Description = x.Description,
                ImageAbsPath = x.ImageRelativePath,
                Id = x.NotificationId,
                ValidTill = x.ValidTill
            })
            .ToArray();
    }
}

