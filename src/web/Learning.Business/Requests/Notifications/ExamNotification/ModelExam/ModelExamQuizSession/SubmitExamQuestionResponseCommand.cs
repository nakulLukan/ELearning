using System.Net;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class SubmitExamQuestionResponseCommand : SubmitExamQuestionResponseRequestDto, IRequest<ResponseDto<long>>
{
    public required long ModelExamResultId { get; set; }
}
public class SubmitExamQuestionResponseCommandHandler : IRequestHandler<SubmitExamQuestionResponseCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly ILogger<SubmitExamQuestionResponseCommandHandler> _logger;

    public SubmitExamQuestionResponseCommandHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext,
        ILogger<SubmitExamQuestionResponseCommandHandler> logger)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _logger = logger;
    }


    public async Task<ResponseDto<long>> Handle(SubmitExamQuestionResponseCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId().ConfigureAwait(false);
        var modelExamResult = await _dbContext.ModelExamResults
            .Where(x => x.Id == request.ModelExamResultId && x.UserId == userId)
            .Select(x => new
            {
                x.StartedOn,
                TotalTimeLimit = x.ExamConfig!.TotalTimeLimit,
                ModelExamResultDetailId = x.ModelExamResultDetails != null ? x.ModelExamResultDetails!.Single(x => x.QuestionId == request.QuestionId).Id : default(long?),
            }).SingleOrDefaultAsync(cancellationToken);
        if (modelExamResult == null)
        {
            throw new AppApiException(HttpStatusCode.NotFound, "ME3001", "Unknown model exam result");
        }

        if ((AppDateTime.UtcNow - modelExamResult.StartedOn).TotalSeconds > modelExamResult.TotalTimeLimit + 10)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "ME3000", "Times Up");
        }

        long? modelExamResultDetailId = modelExamResult.ModelExamResultDetailId;
        if (modelExamResult.ModelExamResultDetailId == null)
        {
            ModelExamResultDetail detail = new()
            {
                ModelExamResultId = request.ModelExamResultId,
                HasSkipped = request.HasSkipped,
                QuestionId = request.QuestionId,
                SelectedAnswerId = request.SelectedAnswerId,
            };

            _dbContext.ModelExamResultDetails.Add(detail);
            await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
            modelExamResultDetailId = detail.Id;
        }
        else
        {
            await _dbContext.ModelExamResultDetails.Where(x => x.Id == modelExamResult.ModelExamResultDetailId.Value)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(y => y.HasSkipped, request.HasSkipped)
                    .SetProperty(y => y.SelectedAnswerId, !request.HasSkipped ? request.SelectedAnswerId : null), cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
        }

        return new(modelExamResultDetailId!.Value);
    }
}

