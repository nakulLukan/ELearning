using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Web.Client.Constants;
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

    [Get("/api/public/exam-notificaitons/{examNotificationId}/model-exams/listing")]
    public Task<GetAllModelExamMetaDataResponseDto[]> GetAllModelExamMetaData(int examNotificationId);

    [Get("/api/public/exam-notificaitons/model-exams/{modelExamId}/validate-subscription")]
    public Task<ResponseDto<bool>> CheckUserModelExamSubscriptionQuery(int modelExamId);
}
