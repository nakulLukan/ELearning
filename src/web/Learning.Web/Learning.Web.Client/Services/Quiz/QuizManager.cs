using Learning.Shared.Common.Utilities;
using Learning.Shared.Enums;
using Learning.Web.Client.Contracts.Services.Quiz;
using Learning.Web.Client.Dto.Quiz;
using System.Text.Json;

namespace Learning.Web.Client.Services.Quiz;

public class QuizManager : IQuizManager
{
    private ILogger<QuizManager> _logger;

    public QuizManager(ILogger<QuizManager> logger)
    {
        _logger = logger;
    }

    private const string encryptionKey = "quiz@1232";

    public QuizAttempStatusEnum GetQuizStatus(string? encryptedData)
    {
        if (encryptedData == null)
        {
            return QuizAttempStatusEnum.NotAttended;
        }

        try
        {
            var jsonData = CryptoEngine.DecryptText(encryptedData, encryptionKey);
            var data = System.Text.Json.JsonSerializer.Deserialize<QuizLocalStorageModel>(jsonData);
            return data?.Status ?? QuizAttempStatusEnum.NotAttended;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse the encrypted data");
            return QuizAttempStatusEnum.NotAttended;
        }
    }

    public byte[] BeginQuiz(string? encryptedData)
    {
        try
        {
            var jsonData = CryptoEngine.DecryptText(encryptedData ?? string.Empty, encryptionKey);
            var data = System.Text.Json.JsonSerializer.Deserialize<QuizLocalStorageModel>(jsonData);
            if (data == null)
            {
                throw new ArgumentNullException(encryptedData);
            }
            data.Status = QuizAttempStatusEnum.Started;
            return CryptoEngine.EncryptText(JsonSerializer.Serialize(data), encryptionKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse the encrypted data");
            var data = new QuizLocalStorageModel()
            {
                Status = QuizAttempStatusEnum.Error
            };
            return CryptoEngine.EncryptText(JsonSerializer.Serialize(data), encryptionKey);
        }
    }

    public QuizLocalStorageModel FinishQuiz(QuizLocalStorageModel model, int quizMaxDiscount)
    {

        model.Status = QuizAttempStatusEnum.Completed;
        model.TotalDiscount = (int)Math.Ceiling(((double)model.MarkScored / model.TotalMark) * quizMaxDiscount);
        model.DiscountCode = CouponCodeGenerator.GenerateCouponCode();
        return model;
    }

    private static byte[] GenerateEncryptedResponse(QuizLocalStorageModel data)
    {
        return CryptoEngine.EncryptText(JsonSerializer.Serialize(data), encryptionKey);
    }

    public (int? Score, int? Discount, string? DiscountCode) GetQuizResult(string encryptedData)
    {
        try
        {
            QuizLocalStorageModel? data = GetQuizData(encryptedData);
            return (data.MarkScored, data.TotalDiscount, data.DiscountCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse the encrypted data");
            return (null, null, null);
        }
    }

    private static QuizLocalStorageModel GetQuizData(string encryptedData)
    {
        var jsonData = CryptoEngine.DecryptText(encryptedData, encryptionKey);
        var data = JsonSerializer.Deserialize<QuizLocalStorageModel>(jsonData);
        return data ?? new();
    }

    public QuizLocalStorageModel SubmitQuestionScore(QuizLocalStorageModel model, int currentQuestionNumber, int score)
    {
        model.CurrentQuestionNumber = currentQuestionNumber;
        model.MarkScored += score;

        return model;
    }
}
