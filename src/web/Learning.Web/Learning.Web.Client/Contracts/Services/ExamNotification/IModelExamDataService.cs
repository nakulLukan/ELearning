using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;

namespace Learning.Web.Client.Contracts.Services.ExamNotification;

public interface IModelExamDataService
{
    public Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId);
    public Task<Result<ResponseDto<bool>>> CheckUserModelExamSubscriptionQuery(int modelExamId);
    public Task<Result<ResponseDto<long>>> InitiateModelExamOrder(int modelExamId);
    public Task<Result<ModeExamOrderCompleteResponseDto>> CompleteModelExamOrder(long modelExamOrderId, OrderStatusEnum status);

}
