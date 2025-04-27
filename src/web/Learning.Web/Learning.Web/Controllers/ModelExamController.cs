using Learning.Business.Requests.ModelExams;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;
using Learning.Business.Requests.PurchaseHistory;
using Learning.Shared.Common.Enums;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

[Route("api/public")]
public class ModelExamController : ControllerBase
{
    private readonly IMediator _mediator;

    public ModelExamController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("model-exams/active")]
    public async Task<IActionResult> GetActiveModelExams()
    {
        var data = await _mediator.Send(new GetActiveModelExamPackagesQuery()).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exams-results/{modelExamResultId:int}/questions/{questionId:int}")]
    public async Task<IActionResult> GetModelExamQuestionById([FromRoute] int modelExamResultId, [FromRoute] int questionId)
    {
        var data = await _mediator.Send(new GetModelExamQuestionByIdQuery()
        {
            ModelExamResultId = modelExamResultId,
            QuestionId = questionId
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpPost("model-exams-results/{modelExamResultId:int}/submit-question-response")]
    public async Task<IActionResult> SubmitExamQuestionResponse([FromRoute] int modelExamResultId, [FromBody] SubmitExamQuestionResponseRequestDto request)
    {
        var data = await _mediator.Send(new SubmitExamQuestionResponseCommand()
        {
            ModelExamResultId = modelExamResultId,
            QuestionId = request.QuestionId,
            HasSkipped = request.HasSkipped,
            SelectedAnswerId = request.SelectedAnswerId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpPost("model-exams-results/{modelExamResultId:int}/complete-session")]
    public async Task<IActionResult> CompleteModelExamSession([FromRoute] int modelExamResultId, [FromQuery] ModelExamSessionStatusEnum status)
    {
        var data = await _mediator.Send(new CompleteModelExamSessionCommand()
        {
            ModelExamResultId = modelExamResultId,
            Status = status
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exams-results/{modelExamResultId:long}/summary")]
    public async Task<IActionResult> GetModelExamSummary([FromRoute] long modelExamResultId)
    {
        var data = await _mediator.Send(new GetModelExamSummaryQuery()
        {
            ModelExamResultId = modelExamResultId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpDelete("model-exams-results/{modelExamResultId:long}")]
    public async Task<IActionResult> DeleteModelExamSession([FromRoute] long modelExamResultId)
    {
        var data = await _mediator.Send(new DeleteModelExamSessionCommand()
        {
            ModelExamResultId = modelExamResultId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exams/{modelExamId:int}/exam-notification-detail")]
    public async Task<IActionResult> GetExamNotificationDetailByModelExamId([FromRoute] int modelExamId)
    {
        var data = await _mediator.Send(new GetExamNotificationDetailByModelExamIdQuery()
        {
            ModelExamId = modelExamId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exam-orders/{modelExamOrderId:long}")]
    public async Task<IActionResult> GetExamNotificationDetailByModelExamId([FromRoute] long modelExamOrderId)
    {
        var data = await _mediator.Send(new GetModelExamOrderByIdQuery()
        {
            ModelExamOrderId = modelExamOrderId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpPost("model-exam-orders/{modelExamOrderId:long}/create-razorpay-order")]
    public async Task<IActionResult> CreateRazorpayOrder([FromRoute] long modelExamOrderId)
    {
        var data = await _mediator.Send(new CreateRazorpayOrderCommand()
        {
            ModelExamOrderId = modelExamOrderId,
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpGet("model-exam-orders/purchase-history")]
    public async Task<IActionResult> GetModelExamPurchaseHistory()
    {
        var data = await _mediator.Send(new ModelExamPurchaseHistoryQuery()
        {
        }).ConfigureAwait(false);
        return Ok(data);
    }

    [Authorize]
    [HttpDelete("model-exam-orders/{modelExamOrderId:long}/purchase-history/delete-failed-order")]
    public async Task<IActionResult> DeleteFailedModelExamOrder([FromRoute] long modelExamOrderId)
    {
        var data = await _mediator.Send(new DeleteFailedOrderCommand()
        {
            ModelExamOrderId = modelExamOrderId
        }).ConfigureAwait(false);
        return Ok(data);
    }
}
