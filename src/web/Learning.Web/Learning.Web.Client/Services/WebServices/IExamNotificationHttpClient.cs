using Learning.Shared.Dto.Notifications.ExamNotification;
using Refit;

namespace Learning.Web.Client.Services.WebServices;

public interface IExamNotificationHttpClient
{
    [Get("/api/public/exam-notificaitons/homepage/active")]
    public Task<ActiveHomepageExamNotificationsQueryResponseDto[]> GetActiveHomePageExamNotifications();

    [Get("/api/public/exam-notificaitons/active")]
    public Task<ExamNotificationCardItemDto[]> GetAllActiveExamNotifications();

    [Get("/api/public/exam-notificaitons/{examNotificationId}")]
    public Task<ActiveExamNotificationDetailDto> ActiveExamNotificationDetailById(int examNotificationId);
}
