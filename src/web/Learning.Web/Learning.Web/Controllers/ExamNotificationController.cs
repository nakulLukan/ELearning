using Learning.Business.Requests.Notifications.ExamNotification;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

[Route("api/public/exam-notificaitons")]
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

    [HttpGet("active")]
    public async Task<IActionResult> GetAllActiveExamNotifications()
    {
        var data = await _mediator.Send(new ActiveExamNotificationsQuery());
        return Ok(data);
    }

    [HttpGet("{examNotificationId:int}")]
    public async Task<IActionResult> GetAllActiveExamNotifications(int examNotificationId)
    {
        var data = await _mediator.Send(new ActiveExamNotificationDetailByIdQuery() { ExamNotificationId = examNotificationId });
        return Ok(data);
    }

    [HttpGet("{examNotificationId:int}/model-exams/listing")]
    public async Task<IActionResult> GetAllModelExamMetaData(int examNotificationId)
    {
        var data = await _mediator.Send(new GetAllModelExamMetaDataQuery() { ExamNotificationId = examNotificationId });
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exams/{modelExamId:int}/validate-subscription")]
    public async Task<IActionResult> CheckUserModelExamSubscription(int modelExamId)
    {
        var data = await _mediator.Send(new CheckUserModelExamSubscriptionQuery() { ModelExamId = modelExamId });
        return Ok(data);
    }
}
