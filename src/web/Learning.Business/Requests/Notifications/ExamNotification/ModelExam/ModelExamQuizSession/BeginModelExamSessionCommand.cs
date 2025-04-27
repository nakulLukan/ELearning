using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Constants;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class BeginModelExamSessionCommand : IRequest<BeginModelExamResponseDto>
{
    public required int ModelExamId { get; set; }
}
public class BeginModelExamSessionCommandHandler : IRequestHandler<BeginModelExamSessionCommand, BeginModelExamResponseDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly ILogger<BeginModelExamSessionCommandHandler> _logger;

    public BeginModelExamSessionCommandHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext,
        ILogger<BeginModelExamSessionCommandHandler> logger)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _logger = logger;
    }

    #region DTO
    private class SessionRecordDto
    {
        public required long ExamResultId { get; set; }
        public required DateTimeOffset StartedOn { get; set; }
        public required DateTimeOffset? CompletedOn { get; set; }
        public required ModelExamSessionStatusEnum Status { get; set; }
        public required ModelExamResultDetailRecordDto[] AttemptedQuestions { get; set; }
    }

    private class ModelExamResultDetailRecordDto
    {
        public required int QuestionId { get; set; }
        public required int? SelectedAnswerId { get; set; }
        public required bool HasSkipped { get; set; }
        public required int Order { get; set; }
    }
    #endregion

    public async Task<BeginModelExamResponseDto> Handle(BeginModelExamSessionCommand request, CancellationToken cancellationToken)
    {
        // Get the exam details
        var userId = await _requestContext.GetUserId().ConfigureAwait(false);
        var examDetails = await (from mec in _dbContext.ModelExamConfigurations.IgnoreQueryFilters()
                                 join mep in _dbContext.ModelExamPackages.IgnoreQueryFilters()
                                     on mec.ModelExamPackageId equals mep.Id into mepGroup
                                 from mep in mepGroup.DefaultIfEmpty()

                                 join meph in _dbContext.ModelExamPurchaseHistory.IgnoreQueryFilters()
                                    .Where(x => x.ModelExamOrder!.UserId == userId 
                                            && x.ValidTill >= DateTimeOffset.UtcNow 
                                            && x.ModelExamOrder!.Status == OrderStatusEnum.Success)
                                     on mep.Id equals meph.ModelExamOrder!.ModelExamPackageId into mephGroup
                                 from meph in mephGroup.DefaultIfEmpty()

                                 where mec.Id == request.ModelExamId
                                 select new
                                 {
                                     OrderValidTill = meph != null ? (DateTimeOffset?)meph.ValidTill : null,
                                     IsFree = mec.IsFree,
                                     TotalQuestions = _dbContext.ModelExamQuestionConfigurations
                                                         .Count(x => x.ExamConfigId == mec.Id),
                                     TotalTimeInSeconds = mec.TotalTimeLimit,
                                     ExamName = mec.ExamName
                                 }).FirstAsync(cancellationToken);

        // The user should be able to attend the exam only if the exam is free or exam package has been purchased
        if (!examDetails.IsFree && (!examDetails.OrderValidTill.HasValue || examDetails.OrderValidTill < DateTimeOffset.UtcNow))
        {
            throw new AppApiException(System.Net.HttpStatusCode.BadRequest, ApiErrorCodes.BME01, "This model exam is not available for you. Please purchase the model exam package to access this model exam.");
        }

        // Check if an session for given model exam is already present
        SessionRecordDto? existingSession = await GetExistingSession(request, userId, cancellationToken);
        ModelExamSessionStatusEnum sessionStatus = ModelExamSessionStatusEnum.Inprogress;
        int? currentQuestionId = null;
        if (existingSession == null)
        {
            // If the user is opening this exam for the first time then create new session
            existingSession = await CreateAndSaveNewSession(request, userId, existingSession, cancellationToken).ConfigureAwait(false);
            sessionStatus = existingSession.Status;
            // Get the current question id
            currentQuestionId = await GetFirstQuestion(request.ModelExamId, cancellationToken);
        }
        else
        {
            var sessionEndsOn = existingSession.StartedOn.AddSeconds(examDetails.TotalTimeInSeconds);
            _logger.LogInformation("Session: {@SessionId}/{@UserId} ends on {@SessionEndsOn}", existingSession.ExamResultId, userId, sessionEndsOn);
            if (existingSession.CompletedOn.HasValue
                && (existingSession.Status == ModelExamSessionStatusEnum.Timeout
                    || existingSession.Status == ModelExamSessionStatusEnum.Completed))
            {
                sessionStatus = existingSession.Status;
                // Current questionId does not have any relevance if the session is completed or got timeout.
                currentQuestionId = 0;
            }
            else if (AppDateTime.UtcNow > sessionEndsOn)
            {
                // Set the status as timeout
                sessionStatus = ModelExamSessionStatusEnum.Timeout;
                await _dbContext.ModelExamResults.Where(x => x.Id == existingSession.ExamResultId)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(y => y.Status, sessionStatus)
                        .SetProperty(y => y.CompletedOn, AppDateTime.UtcNow), cancellationToken).ConfigureAwait(false);
                await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);

                // Current questionId does not have any relevance if the session is completed or got timeout.
                currentQuestionId = 0;
            }
            else
            {
                // If the status is in progress but the user is accessing the second time or more.
                sessionStatus = existingSession.Status;
                var lastAttemptedQuestion = existingSession.AttemptedQuestions.OrderBy(x => x.Order).LastOrDefault(x => x.HasSkipped || x.SelectedAnswerId.HasValue);
                var indexOfLastAttemptedQuestion = lastAttemptedQuestion != null ? existingSession.AttemptedQuestions.ToList().IndexOf(lastAttemptedQuestion) : existingSession.AttemptedQuestions.Length;
                currentQuestionId = existingSession.AttemptedQuestions.Any() ? existingSession.AttemptedQuestions[indexOfLastAttemptedQuestion].QuestionId : await GetFirstQuestion(request.ModelExamId, cancellationToken);
            }
        }

        return new BeginModelExamResponseDto
        {
            ModelExamResultId = existingSession!.ExamResultId,
            StartedOn = existingSession.StartedOn,
            UtcNow = AppDateTime.UtcNow,
            TotalQuestions = examDetails.TotalQuestions,
            TotalTimeInSeconds = examDetails.TotalTimeInSeconds,
            Status = sessionStatus,
            CurrentQuestionId = currentQuestionId!.Value,
            TotalQuestionsAttempted = existingSession.AttemptedQuestions.Length,
            ExamName = examDetails.ExamName
        };
    }

    private async Task<int> GetFirstQuestion(int modelExamId, CancellationToken cancellationToken)
    {
        return await _dbContext.ModelExamQuestionConfigurations
                        .Where(x => x.ExamConfigId == modelExamId)
                        .OrderBy(x => x.Order)
                        .Select(x => x.Id)
                        .FirstAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<SessionRecordDto> CreateAndSaveNewSession(BeginModelExamSessionCommand request, string userId, SessionRecordDto? existingSession, CancellationToken cancellationToken)
    {
        ModelExamResult newSession = new Domain.Notification.ModelExamResult
        {
            ExamConfigId = request.ModelExamId,
            CreatedOn = AppDateTime.UtcNow,
            LastUpdatedOn = AppDateTime.UtcNow,
            StartedOn = AppDateTime.UtcNow,
            UserId = userId,
            Status = ModelExamSessionStatusEnum.Inprogress,
        };
        _dbContext.ModelExamResults.Add(newSession);
        await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
        existingSession = new SessionRecordDto
        {
            AttemptedQuestions = [],
            CompletedOn = null,
            ExamResultId = newSession.Id,
            StartedOn = newSession.StartedOn,
            Status = newSession.Status
        };
        return existingSession;
    }

    private async Task<SessionRecordDto?> GetExistingSession(BeginModelExamSessionCommand request, string userId, CancellationToken cancellationToken)
    {
        return await _dbContext.ModelExamResults
                    .Where(x => x.UserId == userId
                        && x.ExamConfigId == request.ModelExamId)
                    .Select(x => new SessionRecordDto
                    {
                        ExamResultId = x.Id,
                        StartedOn = x.StartedOn,
                        CompletedOn = x.CompletedOn,
                        Status = x.Status,
                        AttemptedQuestions = x.ModelExamResultDetails != null ? x.ModelExamResultDetails!
                                        .Select(y => new ModelExamResultDetailRecordDto
                                        {
                                            QuestionId = y.QuestionId,
                                            SelectedAnswerId = y.SelectedAnswerId,
                                            HasSkipped = y.HasSkipped,
                                            Order = y.Question!.Order
                                        })
                                        .OrderBy(x => x.Order).ToArray() : new ModelExamResultDetailRecordDto[0]
                    })
                    .FirstOrDefaultAsync(cancellationToken);
    }
}

