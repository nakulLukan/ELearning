using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
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

    [Post("/api/public/exam-notificaitons/model-exams/{modelExamId}/initiate-order")]
    public Task<ResponseDto<long>> InitiateModelExamOrder(int modelExamId);

    [Post("/api/public/exam-notificaitons/model-exams-orders/{modelExamOrderId}/complete-order")]
    public Task<ModeExamOrderCompleteResponseDto> CompleteModelExamOrder(long modelExamOrderId, [Query] OrderStatusEnum status);
}
