using FluentResults;
using Learning.Business.Requests.Notifications.ExamNotification;
using Learning.Shared.Dto.Notifications.ExamNotification;
using Learning.Web.Client.Contracts.Services.ExamNotification;
using MediatR;

namespace Learning.Web.Services.ExamNotification;

public class ExamNotificationDataService : IExamNotificationDataService
{
    private readonly IMediator _mediator;

    public ExamNotificationDataService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<ActiveHomepageExamNotificationsQueryResponseDto[]>> GetActiveHomePageExamNotifications()
    {
        try
        {
            var examNotifications = await _mediator.Send(new ActiveHomepageExamNotificationsQuery());
            return examNotifications;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
