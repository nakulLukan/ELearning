using FluentResults;
using Learning.Shared.Dto.Quiz.QuickTest.Public;

namespace Learning.Web.Client.Contracts.Services.Quiz;

public interface IQuizDataService
{
    public Task<Result<QuizMetaDataDto>> GetQuizMetaData();
    public Task<Result<QuizQuestionDto>> GetQuestionByNumber(int quizConfigId, int questionNumber);
}
