using Blazored.LocalStorage;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Enums;
using Learning.Web.Client.Constants;
using Learning.Web.Client.Contracts.Persistance;
using Learning.Web.Client.Contracts.Services.Quiz;
using Learning.Web.Client.Dto.Quiz;
using System.Text.Json;

namespace Learning.Web.Client.Services.Quiz;

public class QuizManager : IQuizManager
{
    private readonly ILogger<QuizManager> _logger;
    private readonly IBrowserStorage _browserStorage;
    private readonly ILocalStorageService _localStorage;


    public QuizManager(ILogger<QuizManager> logger, IBrowserStorage browserStorage, ILocalStorageService localStorage)
    {
        _logger = logger;
        _browserStorage = browserStorage;
        _localStorage = localStorage;
    }

    private const string encryptionKey = "quiz@1232";

    public async Task<QuizLocalStorageModel> GetQuizModel()
    {
        try
        {
            return await _browserStorage.Get<QuizLocalStorageModel>(BrowserStorageKeys.QuizDataLocalStorageKey, BrowserStorageKeys.QuizDataLocalStorageEncryptionKey) ?? new();
        }
        catch
        {
            return new QuizLocalStorageModel();
        }
    }

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
        model.DiscountCode = "V" + model.QuizVersionNumber + "-" + CouponCodeGenerator.GenerateCouponCode();
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

    public async Task<DateOnly?> WhenQuizLatestInfoWasChecked()
    {
        if (!await _localStorage.ContainKeyAsync(BrowserStorageKeys.QuizLatestInfoLastCheckedStorageKey))
        {
            return null;
        }
        return await _localStorage.GetItemAsync<DateOnly>(BrowserStorageKeys.QuizLatestInfoLastCheckedStorageKey);
    }

    public async Task SetWhenQuizLatestInfoWasChecked(DateOnly value)
    {
        await _localStorage.SetItemAsync(BrowserStorageKeys.QuizLatestInfoLastCheckedStorageKey, value);
    }
}
