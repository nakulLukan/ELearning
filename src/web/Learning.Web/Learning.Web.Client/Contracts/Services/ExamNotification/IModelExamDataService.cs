using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

namespace Learning.Web.Client.Contracts.Services.ExamNotification;

public interface IModelExamDataService
{
    public Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId);
    public Task<Result<ResponseDto<bool>>> CheckUserModelExamSubscriptionQuery(int modelExamId);
    public Task<Result<ResponseDto<long>>> InitiateModelExamOrder(int modelExamId);
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
}
