using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.ModelExam.Payment;
using Learning.Shared.Dto.ModelExams;
using Learning.Shared.Dto.ModelExams.Payment;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Learning.Shared.Dto.PurchaseHistory;

namespace Learning.Web.Client.Contracts.Services.ExamNotification;

public interface IModelExamDataService
{
    public Task<Result<ModelExamPurchaseNowDto>> GetModelExamPurchaseDetails(int examNotificationId);
    public Task<Result<ActiveModelExamPackageBasicDetailDto[]>> GetActiveModelExams();
    public Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId);
    public Task<Result<ResponseDto<bool>>> CheckUserModelExamSubscriptionQuery(int modelExamId);
    public Task<Result<ResponseDto<long>>> InitiateModelExamOrder(int examNotificationId);
    public Task<Result<ModeExamOrderCompleteResponseDto>> CompleteModelExamOrder(long modelExamOrderId, OrderStatusEnum status);
    public Task<Result<BeginModelExamResponseDto>> BeginModelExam(int modelExamId);
    public Task<Result<GetExamQuestionsListItemResponseDto[]>> GetExamQuestionsList(int modelExamId);
    public Task<Result<ModelExamSessionQuestionDetailDto>> GetModelExamQuestionById(long modelExamResultId, int questionId);
    public Task<Result<ResponseDto<long>>> SubmitExamQuestionResponse(
        long modelExamResultId,
        int questionId,
        int? selectedAnswerId,
        bool hasSkipped);
    public Task<Result<ResponseDto<ModelExamSessionStatusEnum>>> CompleteModelExamSession(long modelExamResultId, ModelExamSessionStatusEnum status);
    public Task<Result<GetModelExamSummaryResponseDto>> GetModelExamSummary(long modelExamResultId);
    public Task<Result<ResponseDto<bool>>> DeleteModelExamSession(long modelExamResultId);
    public Task<Result<ExamNotificationDetailResponseDto>> GetExamNotificationDetailByModelExamId(int modelExamId);
    public Task<Result<ModelExamOrderStepDetailDto>> GetModelExamOrderById(long modelExamOrderId);
    public Task<Result<ModelExamOrderStepDetailDto>> CreateRazorpayOrder(long modelExamOrderId);
    public Task<Result<ModelExamPaymentReceipt>> GetPaymentReceipt(long modelExamOrderId);
    public Task<Result<ModelExamPurchaseHistoryItemDto[]>> GetModelExamPurchaseHistory();

}
