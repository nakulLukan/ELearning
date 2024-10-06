using Learning.Business.Requests.Notifications.ExamNotification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

[Route("api/public/exam-notificaiton")]
public class ExamNotificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamNotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("homepage/active")]
    public async Task<IActionResult> GetActiveHomePageExamNotifications()
    {
        var data = await _mediator.Send(new ActiveHomepageExamNotificationsQuery());
        return Ok(data);
    }
}
