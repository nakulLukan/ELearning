using FluentResults;
using Learning.Business.Requests.Quiz.QuickTest.Public;
using Learning.Shared.Dto.Quiz.QuickTest.Public;
using Learning.Web.Client.Contracts.Services.Quiz;
using MediatR;

namespace Learning.Web.Services.Quiz;

public class QuizDataService : IQuizDataService
{
    private readonly IMediator _mediator;
    public QuizDataService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<QuizMetaDataDto>> GetQuizMetaData()
    {
        var data = await _mediator.Send(new GetQuizMetaDataQuery());
        return data;
    }

    public async Task<Result<QuizQuestionDto>> GetQuestionByNumber(int quizConfigId, int questionNumber)
    {
        return await _mediator.Send(new GetQuizQuestionByNumberQuery()
        {
            QuestionNumber = questionNumber,
            QuizConfigId = quizConfigId,
        });
    }
}
