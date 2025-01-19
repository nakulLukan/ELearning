using FluentResults;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Contracts.HttpContext;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
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

    public async Task<Result<BeginModelExamResponseDto>> BeginModelExam(int modelExamId)
    {
        try
        {
            var result = await _httpClient.BeginModelExam(modelExamId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<GetExamQuestionsListItemResponseDto[]>> GetExamQuestionsList(int modelExamId)
    {
        try
        {
            var result = await _httpClient.GetExamQuestionsList(modelExamId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ModelExamSessionQuestionDetailDto>> GetModelExamQuestionById(long modelExamResultId, int questionId)
    {
        try
        {
            var result = await _httpClient.GetModelExamQuestionById(modelExamResultId, questionId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<long>>> SubmitExamQuestionResponse(
        long modelExamResultId,
        int questionId,
        int? selectedAnswerId,
        bool hasSkipped)
    {
        try
        {
            var result = await _httpClient.SubmitExamQuestionResponse(modelExamResultId, new()
            {
                HasSkipped = hasSkipped,
                QuestionId = questionId,
                SelectedAnswerId = selectedAnswerId
            });
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ResponseDto<ModelExamSessionStatusEnum>>> CompleteModelExamSession(long modelExamResultId, ModelExamSessionStatusEnum status)
    {
        try
        {
            var result = await _httpClient.CompleteModelExamSession(modelExamResultId, status);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<GetModelExamSummaryResponseDto>> GetModelExamSummary(long modelExamResultId)
    {
        try
        {
            var result = await _httpClient.GetModelExamSummary(modelExamResultId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
