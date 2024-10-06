using FluentResults;
using Learning.Shared.Dto.Notifications.ExamNotification;

namespace Learning.Web.Client.Contracts.Services.ExamNotification;

public interface IExamNotificationDataService
{
    public Task<Result<ActiveHomepageExamNotificationsQueryResponseDto[]>> GetActiveHomePageExamNotifications();
}
