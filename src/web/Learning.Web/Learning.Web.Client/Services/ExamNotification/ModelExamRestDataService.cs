using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Contracts.HttpContext;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Web.Client.Contracts.Services.ExamNotification;
using Learning.Web.Client.Services.WebServices;

namespace Learning.Web.Client.Services.ExamNotification;

public class ModelExamRestDataService : IModelExamDataService
{
    private readonly IExamNotificationHttpClient _httpClient;
    private readonly IRequestContext _requestContext;

    public ModelExamRestDataService(
        IExamNotificationHttpClient httpClient,
        IRequestContext requestContext)
    {
        _httpClient = httpClient;
        _requestContext = requestContext;
    }

    public async Task<Result<GetAllModelExamMetaDataResponseDto[]>> GetAllModelExamMetaData(int examNotificationId)
    {
        try
        {
            var result = await _httpClient.GetAllModelExamMetaData(examNotificationId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<bool>>> CheckUserModelExamSubscriptionQuery(int modelExamId)
    {
        try
        {
            var result = await _httpClient.CheckUserModelExamSubscriptionQuery(modelExamId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<long>>> InitiateModelExamOrder(int modelExamId)
    {
        try
        {
            var result = await _httpClient.InitiateModelExamOrder(modelExamId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModeExamOrderCompleteResponseDto>> CompleteModelExamOrder(long modelExamOrderId, OrderStatusEnum status)
    {
        try
        {
            var result = await _httpClient.CompleteModelExamOrder(modelExamOrderId, status);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
