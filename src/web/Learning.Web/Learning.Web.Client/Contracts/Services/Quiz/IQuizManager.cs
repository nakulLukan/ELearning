using Learning.Shared.Enums;
using Learning.Web.Client.Dto.Quiz;

namespace Learning.Web.Client.Contracts.Services.Quiz;

public interface IQuizManager
{
    public Task<QuizLocalStorageModel> GetQuizModel();
    public QuizAttempStatusEnum GetQuizStatus(string? encryptedData);
    public byte[] BeginQuiz(string? encryptedData);

    public QuizLocalStorageModel FinishQuiz(QuizLocalStorageModel model, int quizMaxDiscount);
    public (int? Score, int? Discount, string? DiscountCode) GetQuizResult(string encryptedData);
    public QuizLocalStorageModel SubmitQuestionScore(QuizLocalStorageModel model, int currentQuestionNumber, int score);
}
