using Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
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
    public async Task<IActionResult> CompleteModelExamSession([FromRoute] int modelExamResultId)
    {
        var data = await _mediator.Send(new CompleteModelExamSessionCommand()
        {
            ModelExamResultId = modelExamResultId,
        }).ConfigureAwait(false);
        return Ok(data);
    }
}
