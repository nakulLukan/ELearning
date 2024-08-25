using FluentResults;
using Learning.Shared.Dto.Quiz.QuickTest.Public;
using Learning.Web.Client.Contracts.Services.Quiz;
using Learning.Web.Client.Services.WebServices;

namespace Learning.Web.Client.Services.Quiz;

public class QuizRestDataService : IQuizDataService
{
    private readonly IPublicQuizRestService _httpClient;

    public QuizRestDataService(IPublicQuizRestService httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<QuizQuestionDto>> GetQuestionByNumber(int quizConfigId, int questionNumber)
    {
        try
        {
            var result = await _httpClient.GetQuestionByNumber(quizConfigId, questionNumber);
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<Result<QuizMetaDataDto>> GetQuizMetaData()
    {
        try
        {
            var result = await _httpClient.GetQuizMetaData();
            return Result.Ok(result);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}
