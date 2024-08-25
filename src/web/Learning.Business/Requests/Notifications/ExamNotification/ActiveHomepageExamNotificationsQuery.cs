using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification;
using MediatR;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class ActiveHomepageExamNotificationsQuery : IRequest<ActiveHomepageExamNotificationsQueryResponseDto[]>
{
}

public class ActiveHomepageExamNotificationsQueryHandler : IRequestHandler<ActiveHomepageExamNotificationsQuery, ActiveHomepageExamNotificationsQueryResponseDto[]>
{
    private readonly IExamNotificationManager _examNotificationManager;

    public ActiveHomepageExamNotificationsQueryHandler(
        IExamNotificationManager examNotificationManager)
    {
        _examNotificationManager = examNotificationManager;
    }

    public async Task<ActiveHomepageExamNotificationsQueryResponseDto[]> Handle(ActiveHomepageExamNotificationsQuery request, CancellationToken cancellationToken)
    {
        var examNotifications = await _examNotificationManager.GetAllActiveExamNotifications(cancellationToken);

        return (examNotifications ?? [])
            .Select(x => new ActiveHomepageExamNotificationsQueryResponseDto
            {
                Title = x.Title,
                Description = x.Description,
                ImagePath = x.ImageRelativePath,
                NotificationId = x.NotificationId
            })
            .ToArray();
    }
}

