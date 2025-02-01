using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class GetModelExamSummaryQuery : IRequest<GetModelExamSummaryResponseDto>
{
    public required long ModelExamResultId { get; set; }
}
public class GetModelExamSummaryQueryHandler : IRequestHandler<GetModelExamSummaryQuery, GetModelExamSummaryResponseDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly IFileStorage _fileStorage;

    public GetModelExamSummaryQueryHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<GetModelExamSummaryResponseDto> Handle(GetModelExamSummaryQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var sessionDetail = await _dbContext.ModelExamResults
            .Where(x => x.Id == request.ModelExamResultId && x.UserId == userId)
            .Select(x => new
            {
                ModelExamId = x.ExamConfigId,
                ModelExamPackageId = x.ExamConfig!.ModelExamPackageId,
                x.ExamConfig!.ExamName,
                x.Status,
                x.ExamConfig!.TotalTimeLimit,
                x.StartedOn,
                x.CompletedOn,
                Details = x.ModelExamResultDetails != null ? x.ModelExamResultDetails!
                .Select(y => new
                {
                    y.QuestionId,
                    y.Question!.Score,
                    Order = y.Question!.Order,
                    SelectedOptionId = y.SelectedAnswerId,
                    HasSkipped = y.HasSkipped,
                    QuestionText = y.Question.QuestionText,
                    QuestionImageRelativePath = y.Question.QuestionImage != null ? y.Question.QuestionImage.RelativePath : null,
                    Options = y.Question!.ModelExamAnswers!.Select(z => new
                    {
                        OptionId = z.Id,
                        Order = z.Order,
                        OptionText = z.AnswerText,
                        IsCorrectAnswer = z.IsCorrectAnswer,
                        OptionImageRelativePath = z.AnswerImage != null ? z.AnswerImage.RelativePath : null,
                    }).ToArray()
                })
                .ToArray() : null
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (sessionDetail == null)
        {
            throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MES101", "Session detail not found");
        }

        if (sessionDetail.Status == Shared.Common.Enums.ModelExamSessionStatusEnum.Inprogress
            && ((AppDateTime.UtcNow - sessionDetail.StartedOn).TotalSeconds < sessionDetail.TotalTimeLimit + 10))
        {
            throw new AppApiException(System.Net.HttpStatusCode.BadRequest, "MES102", "Session is still in progress");
        }

        // When user ends exam without answering any questions then load the questions from configuration.
        // Mark all questions as skipped.
        var questionIds = sessionDetail.Details?.Select(x => x.QuestionId)?.ToArray() ?? [];
        QuestionSummary[] untouchedQuestions = await GetUnTouchedQuestions(sessionDetail.ModelExamId, questionIds, cancellationToken);

        (int? nextExamId, string? nextExamName) = await GetNextModelExamDetails(
            sessionDetail.ModelExamId,
            sessionDetail.ModelExamPackageId,
            cancellationToken);

        var result = new GetModelExamSummaryResponseDto
        {
            NextModelExamId = nextExamId,
            NextModelExamName = nextExamName,
            ModelExamId = sessionDetail.ModelExamId,
            ExamName = sessionDetail.ExamName,
            Status = sessionDetail.Status,
            TotalTimeLimit = sessionDetail.TotalTimeLimit,
            SessionDurationInSeconds = sessionDetail.CompletedOn.HasValue ? (int)(sessionDetail.CompletedOn.Value - sessionDetail.StartedOn).TotalSeconds : null,
            QuestionSummary = sessionDetail.Details!.Select(x => new QuestionSummary()
            {
                Score = x.Score,
                HasSkipped = x.HasSkipped,
                SelectedOptionId = x.SelectedOptionId,
                QuestionText = x.QuestionText!,
                Order = x.Order,
                QuestionImageUrl = x.QuestionImageRelativePath != null ? _fileStorage.GetPresignedUrl(x.QuestionImageRelativePath) : null,
                OptionSummary = x.Options.Select(y => new OptionSummary()
                {
                    IsCorrectAnswer = y.IsCorrectAnswer,
                    OptionId = y.OptionId,
                    OptionText = y.OptionText,
                    Order = y.Order,
                    OptionImageRelativeUrl = y.OptionImageRelativePath != null ? _fileStorage.GetPresignedUrl(y.OptionImageRelativePath) : null
                }).ToArray()
            }).ToList()
        };

        result.QuestionSummary.AddRange(untouchedQuestions);
        return result;
    }

    private async Task<QuestionSummary[]> GetUnTouchedQuestions(int modelExamId, int[] questionIds, CancellationToken cancellationToken)
    {
        return await _dbContext.ModelExamQuestionConfigurations.Where(x => x.ExamConfigId == modelExamId
                    && !questionIds.Contains(x.Id)).Select(x => new QuestionSummary
                    {
                        HasSkipped = true,
                        OptionSummary = x.ModelExamAnswers != null ? x.ModelExamAnswers!.Select(y => new OptionSummary
                        {
                            IsCorrectAnswer = y.IsCorrectAnswer,
                            OptionId = y.Id,
                            OptionText = y.AnswerText,
                            Order = y.Order,
                            OptionImageRelativeUrl = y.AnswerImage != null ? _fileStorage.GetPresignedUrl(y.AnswerImage.RelativePath) : null,
                        }).ToArray() : new OptionSummary[0],
                        Order = x.Order,
                        QuestionImageUrl = x.QuestionImage != null ? _fileStorage.GetPresignedUrl(x.QuestionImage.RelativePath) : null,
                        QuestionText = x.QuestionText!,
                        Score = x.Score,
                        SelectedOptionId = null
                    })
                    .ToArrayAsync(cancellationToken);
    }

    private async Task<(int? nextExamId, string? nextExamName)> GetNextModelExamDetails(int modelExamId, int modelExamPackageId, CancellationToken cancellationToken)
    {
        var nextModelExams = await _dbContext.ModelExamConfigurations
                    .Where(x => x.ModelExamPackageId == modelExamPackageId
                        && x.Id != modelExamId)
                    .OrderBy(x => x.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.ExamName
                    }).ToArrayAsync(cancellationToken);
        var nextExam = nextModelExams.FirstOrDefault(x => x.Id > modelExamId);
        return (nextExam?.Id, nextExam?.ExamName);
    }
}

