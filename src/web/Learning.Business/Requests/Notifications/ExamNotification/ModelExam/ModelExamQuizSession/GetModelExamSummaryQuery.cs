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
                x.Status,
                x.ExamConfig!.TotalTimeLimit,
                x.StartedOn,
                x.CompletedOn,
                Details = x.ModelExamResultDetails != null ? x.ModelExamResultDetails!
                .Select(y => new
                {
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

        return new GetModelExamSummaryResponseDto
        {
            Status = sessionDetail.Status,
            TotalTimeLimit = sessionDetail.TotalTimeLimit,
            SessionDurationInSeconds = sessionDetail.CompletedOn.HasValue ? (int)(sessionDetail.CompletedOn.Value - sessionDetail.StartedOn).TotalSeconds : null,
            QuestionSummary = sessionDetail.Details!.Select(x => new QuestionSummary()
            {
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
            }).ToArray()
        };
    }
}

