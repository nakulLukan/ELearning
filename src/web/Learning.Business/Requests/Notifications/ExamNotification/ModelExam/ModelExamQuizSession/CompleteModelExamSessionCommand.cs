using System.Net;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class CompleteModelExamSessionCommand : IRequest<ResponseDto<ModelExamSessionStatusEnum>>
{
    public required long ModelExamResultId { get; set; }
}
public class CompleteModelExamSessionCommandHandler : IRequestHandler<CompleteModelExamSessionCommand, ResponseDto<ModelExamSessionStatusEnum>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;

    public CompleteModelExamSessionCommandHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<ModelExamSessionStatusEnum>> Handle(CompleteModelExamSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId().ConfigureAwait(false);
        var modelExamResult = await _dbContext.ModelExamResults.AsTracking()
            .Where(x => x.Id == request.ModelExamResultId && x.UserId == userId)
            .Select(x => new
            {
                x.StartedOn,
                x.ExamConfig!.TotalTimeLimit,
                x.Status
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (modelExamResult == null)
        {
            throw new AppApiException(HttpStatusCode.NotFound, "ME40", "Model exam result not found");
        }

        if (modelExamResult.Status != Shared.Common.Enums.ModelExamSessionStatusEnum.Inprogress)
        {
            throw new AppApiException(HttpStatusCode.BadRequest, "ME41", "Cannot model exam status");
        }

        var status = (AppDateTime.UtcNow - modelExamResult.StartedOn).TotalSeconds >= modelExamResult.TotalTimeLimit + 10 ? ModelExamSessionStatusEnum.Timeout : ModelExamSessionStatusEnum.Completed;

        await _dbContext.ModelExamResults
            .Where(x => x.Id == request.ModelExamResultId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(y => y.Status, status)
                 .SetProperty(y => y.CompletedOn, AppDateTime.UtcNow), cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
        return new(status);
    }
}

