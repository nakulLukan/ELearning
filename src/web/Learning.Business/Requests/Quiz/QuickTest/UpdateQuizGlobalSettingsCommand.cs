using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class UpdateQuizGlobalSettingsCommand : IRequest<ResponseDto<int>>
{
    public required float MinimumPassPercentage { get; set; }
    public required int MaximumDiscountPercentage { get; set; }
    public int? QuizConfigId { get; set; }
}

public class UpdateQuizGlobalSettingsCommandHandler : IRequestHandler<UpdateQuizGlobalSettingsCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;

    public UpdateQuizGlobalSettingsCommandHandler(
        IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ResponseDto<int>> Handle(UpdateQuizGlobalSettingsCommand request, CancellationToken cancellationToken)
    {
        var quizConfig = await _dbContext.QuizConfigurations.AsTracking()
            .SingleOrDefaultAsync(x => (request.QuizConfigId.HasValue && x.Id == request.QuizConfigId)
                || !request.QuizConfigId.HasValue && x.IsDefault) ?? throw new AppException("Invalid quiz configuration id");
        quizConfig.DiscountPercentage = request.MaximumDiscountPercentage;
        quizConfig.PassPercentage = request.MinimumPassPercentage;
        quizConfig.LastUpdatedOn = AppDateTime.UtcNow;

        await _dbContext.SaveAsync(cancellationToken);
        return new(quizConfig.Id);
    }
}

