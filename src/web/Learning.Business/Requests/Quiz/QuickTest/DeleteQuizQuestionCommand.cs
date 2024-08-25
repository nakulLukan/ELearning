using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class DeleteQuizQuestionCommand : IRequest<ResponseDto<bool>>
{
    public required int QuestionId { get; set; }
}

public class DeleteQuizQuestionCommandHandler : IRequestHandler<DeleteQuizQuestionCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public DeleteQuizQuestionCommandHandler(
        IAppDbContextFactory dbContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<bool>> Handle(DeleteQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        // Get default quiz or given quiz id.
        // If quiz id is null then take default quiz
        var quizConfig = await _dbContext.QuizQuestions.AsTracking()
            .FirstAsync(x => x.Id == request.QuestionId, cancellationToken);
        var currentQuestionOrder = quizConfig.Order;
        _dbContext.QuizQuestions.Remove(quizConfig);
        await _dbContext.SaveAsync(cancellationToken);
        await _dbContext.QuizQuestions
            .Where(x => x.QuizConfigurationId == quizConfig.QuizConfigurationId
                && x.Order > currentQuestionOrder)
            .ExecuteUpdateAsync((setters) => setters.SetProperty(y => y.Order, y => y.Order - 1));

        return new(true);
    }
}

