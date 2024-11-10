using FluentResults;
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
}
