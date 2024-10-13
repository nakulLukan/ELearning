using FluentResults;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Web.Client.Contracts.Services.ExamNotification;
using Learning.Web.Client.Services.WebServices;

namespace Learning.Web.Client.Services.ExamNotification;

public class ExamNotificationRestDataService : IExamNotificationDataService
{
    private readonly IExamNotificationHttpClient _httpClient;

    public ExamNotificationRestDataService(IExamNotificationHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<ActiveHomepageExamNotificationsQueryResponseDto[]>> GetActiveHomePageExamNotifications()
    {
        try
        {
            var result = await _httpClient.GetActiveHomePageExamNotifications();
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ExamNotificationCardItemDto[]>> GetAllActiveExamNotifications()
    {
        try
        {
            var result = await _httpClient.GetAllActiveExamNotifications();
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<ActiveExamNotificationDetailDto>> ActiveExamNotificationDetailById(int examNotificationId)
    {
        try
        {
            var result = await _httpClient.ActiveExamNotificationDetailById(examNotificationId);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
