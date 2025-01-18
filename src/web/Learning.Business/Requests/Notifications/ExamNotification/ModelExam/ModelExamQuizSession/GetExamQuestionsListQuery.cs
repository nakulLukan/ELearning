using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using Learning.Business.Impl.Data;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class GetExamQuestionsListQuery : IRequest<GetExamQuestionsListItemResponseDto[]>
{
    public required int ModelExamId { get; set; }
}
public class GetExamQuestionsListQueryHandler : IRequestHandler<GetExamQuestionsListQuery, GetExamQuestionsListItemResponseDto[]>
{
    private readonly IAppDbContext _dbContext;
    private readonly IAppCache _cache;

    public GetExamQuestionsListQueryHandler(
        IAppDbContextFactory dbContext,
        IAppCache cache)
    {
        _dbContext = dbContext.CreateDbContext();
        _cache = cache;
    }

    public async Task<GetExamQuestionsListItemResponseDto[]> Handle(GetExamQuestionsListQuery request, CancellationToken cancellationToken)
    {
        var cacheData = _cache.Get<ModelExamAssociatedQuestionCacheDto>(ExamNotificationCacheKey.ModelExamAssociatedQuestionKey(request.ModelExamId));
        if (cacheData.HasData && cacheData.Data is ModelExamAssociatedQuestionCacheDto associatedQuestions)
        {
            int cacheQuestionNumber = 1;
            return associatedQuestions.QuestionIds.Select(x => new GetExamQuestionsListItemResponseDto
            {
                QuestionId = x,
                QuestionNumber = cacheQuestionNumber++
            }).ToArray();
        }

        var questions = await _dbContext.ModelExamQuestionConfigurations
            .Where(x => x.ExamConfigId == request.ModelExamId
                && x.IsActive)
            .OrderBy(x => x.Order)
            .Select(x => new
            {
                QuestionId = x.Id,
            })
            .ToArrayAsync(cancellationToken).ConfigureAwait(false);

        _cache.SetWithSlidingExpiration(ExamNotificationCacheKey.ModelExamAssociatedQuestionKey(request.ModelExamId), new ModelExamAssociatedQuestionCacheDto
        {
            ModelExamId = request.ModelExamId,
            QuestionIds = questions.Select(x => x.QuestionId).ToArray()
        }, TimeSpan.FromMinutes(15));

        // Return the result after recalculating the question order
        int questionNumber = 1;
        return questions.Select(x => new GetExamQuestionsListItemResponseDto
        {
            QuestionId = x.QuestionId,
            QuestionNumber = questionNumber++
        }).ToArray();
    }
}

