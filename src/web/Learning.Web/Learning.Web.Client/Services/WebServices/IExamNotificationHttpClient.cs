using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.ModelExams;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Refit;

namespace Learning.Web.Client.Services.WebServices;

public interface IExamNotificationHttpClient
{
    [Get("/api/public/exam-notificaitons/{examNotificationId}/model-exam-package/purchase-details")]
    public Task<ModelExamPurchaseNowDto> GetModelExamPurchaseDetails(int examNotificationId);

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

    [Post("/api/public/exam-notificaitons/{examNotificationId}/initiate-order")]
    public Task<ResponseDto<long>> InitiateExamNotificationModelExamPackageOrder(int examNotificationId);

    [Post("/api/public/exam-notificaitons/model-exams-orders/{modelExamOrderId}/complete-order")]
    public Task<ModeExamOrderCompleteResponseDto> CompleteModelExamOrder(long modelExamOrderId, [Query] OrderStatusEnum status);

    [Post("/api/public/exam-notificaitons/model-exams/{modelExamId}/begin")]
    public Task<BeginModelExamResponseDto> BeginModelExam(int modelExamId);

    [Get("/api/public/exam-notificaitons/model-exams/{modelExamId}/associated-questions")]
    public Task<GetExamQuestionsListItemResponseDto[]> GetExamQuestionsList(int modelExamId);

    [Get("/api/public/model-exams-results/{modelExamResultId}/questions/{questionId}")]
    public Task<ModelExamSessionQuestionDetailDto> GetModelExamQuestionById(long modelExamResultId, int questionId);

    [Post("/api/public/model-exams-results/{modelExamResultId}/submit-question-response")]
    public Task<ResponseDto<long>> SubmitExamQuestionResponse(long modelExamResultId, [Body] SubmitExamQuestionResponseRequestDto request);

    [Post("/api/public/model-exams-results/{modelExamResultId}/complete-session")]
    public Task<ResponseDto<ModelExamSessionStatusEnum>> CompleteModelExamSession(long modelExamResultId, [Query] ModelExamSessionStatusEnum status);

    [Get("/api/public/model-exams-results/{modelExamResultId}/summary")]
    public Task<GetModelExamSummaryResponseDto> GetModelExamSummary(long modelExamResultId);

    [Delete("/api/public/model-exams-results/{modelExamResultId}")]
    public Task<ResponseDto<bool>> DeleteModelExamSession(long modelExamResultId);

    [Get("/api/public/model-exams/active")]
    public Task<ActiveModelExamPackageBasicDetailDto[]> GetActiveModelExams();
}
