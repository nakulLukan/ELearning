using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class DeleteModelExamSessionCommand : IRequest<ResponseDto<bool>>
{
    public required long ModelExamResultId { get; set; }
}
public class DeleteModelExamSessionCommandHandler : IRequestHandler<DeleteModelExamSessionCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly ILogger<DeleteModelExamSessionCommandHandler> _logger;

    public DeleteModelExamSessionCommandHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext,
        ILogger<DeleteModelExamSessionCommandHandler> logger)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _logger = logger;
    }

    public async Task<ResponseDto<bool>> Handle(DeleteModelExamSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var count = await _dbContext.ModelExamResults
            .Where(x => x.Id == request.ModelExamResultId
                && x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
        await _dbContext.SaveAsync(cancellationToken);
        return new(count > 0);
    }
}

