﻿using Learning.Business.Requests.ModelExams;
using Learning.Business.Requests.Notifications.ExamNotification;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Learning.Shared.Common.Enums;
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

    [Authorize]
    [HttpPost("{examNotificationId:int}/initiate-order")]
    public async Task<IActionResult> InitiateExamNotificationModelExamPackageOrder([FromRoute] int examNotificationId)
    {
        var data = await _mediator.Send(new ModelExamOrderInitiateCommand() { ExamNotificationId = examNotificationId }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpPost("model-exams-orders/{modelExamOrderId:long}/complete-order")]
    public async Task<IActionResult> CompleteModelExamOrder([FromRoute] long modelExamOrderId, [FromQuery] OrderStatusEnum status)
    {
        var data = await _mediator.Send(new ModelExamOrderCompleteCommand()
        {
            ModelExamOrderId = modelExamOrderId,
            OrderStatus = status
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpPost("model-exams/{modelExamId:int}/begin")]
    public async Task<IActionResult> BeginModelExam([FromRoute] int modelExamId)
    {
        var data = await _mediator.Send(new BeginModelExamSessionCommand()
        {
            ModelExamId = modelExamId
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exams/{modelExamId:int}/associated-questions")]
    public async Task<IActionResult> GetExamQuestionsList([FromRoute] int modelExamId)
    {
        var data = await _mediator.Send(new GetExamQuestionsListQuery()
        {
            ModelExamId = modelExamId
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [HttpGet("{examNotificationId:int}/model-exam-package/purchase-details")]
    public async Task<IActionResult> GetModelExamPurchaseDetails([FromRoute] int examNotificationId)
    {
        var data = await _mediator.Send(new ModelExamPurchaseNowQuery()
        {
            ExamNotificationId = examNotificationId
        }).ConfigureAwait(false);
        return Ok(data);
    }
}
