using Learning.Shared.Common.Dto;
using Learning.Shared.Dto.Quiz.QuickTest.Public;
using Learning.Shared.Dto.Subscription.Offer;
using Refit;

namespace Learning.Web.Client.Services.WebServices;

public interface IPublicQuizRestService
{

    [Get("/api/public/quiz/meta-data")]
    public Task<QuizMetaDataDto> GetQuizMetaData();

    [Get("/api/public/quiz/configs/{quizConfigId}/questions/{questionNumber}")]
    public Task<QuizQuestionDto> GetQuestionByNumber(
        int quizConfigId,
        int questionNumber);

    [Post("/api/public/quiz/save-coupon-code")]
    public Task<ResponseDto<long>> SaveCouponCode([Body] AddCouponCodeCommandRequestDto request);
}
