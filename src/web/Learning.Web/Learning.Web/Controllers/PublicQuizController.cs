using Learning.Business.Requests.Quiz.QuickTest.Public;
using Learning.Business.Requests.Subscription.Offer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Web.Controllers;

[Route("api/public/quiz")]
public class PublicQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public PublicQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("meta-data")]
    public async Task<IActionResult> GetQuizMetaData()
    {
        var data = await _mediator.Send(new GetQuizMetaDataQuery());
        return Ok(data);
    }

    [HttpGet("configs/{quizConfigId:int}/questions/{questionNumber:int}")]
    public async Task<IActionResult> GetQuestionByNumber(
        int quizConfigId,
        [FromRoute] int questionNumber)
    {
        var result = await _mediator.Send(new GetQuizQuestionByNumberQuery()
        {
            QuestionNumber = questionNumber,
            QuizConfigId = quizConfigId,
        });
        return Ok(result);
    }

    [HttpPost("save-coupon-code")]
    public async Task<IActionResult> SaveCouponCode([FromBody] AddCouponCodeCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
