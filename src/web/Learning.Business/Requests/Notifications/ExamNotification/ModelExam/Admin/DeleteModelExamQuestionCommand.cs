using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Dto;
using Learning.Shared.Contracts.HttpContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class DeleteModelExamQuestionCommand : IRequest<ResponseDto<bool>>
{
    public required long ModelExamQuestionId { get; set; }
    public required bool ForceDelete { get; set; }
}
public class DeleteModelExamQuestionCommandHandler : IRequestHandler<DeleteModelExamQuestionCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;

    public DeleteModelExamQuestionCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ResponseDto<bool>> Handle(DeleteModelExamQuestionCommand request, CancellationToken cancellationToken)
    {
        // If question is already attended by any user and the exam is not free. Then ask for a confirmation. For that return false.
        if (!request.ForceDelete)
        {
            var questionAlreadyAttended = await _dbContext.ModelExamResultDetails.AnyAsync(x => x.QuestionId == request.ModelExamQuestionId && !x.Question!.ExamConfig!.IsFree, cancellationToken);
            if (questionAlreadyAttended)
            {
                return new(false);
            }
        }

        // Update the order of other questions
        var question = await _dbContext.ModelExamQuestionConfigurations
            .Where(x => x.Id == request.ModelExamQuestionId)
            .Select(x => new
            {
                x.Order,
                x.ExamConfigId
            })
            .FirstAsync(cancellationToken);

        await _dbContext.ModelExamQuestionConfigurations
            .Where(x => x.ExamConfigId == question!.ExamConfigId && x.Order > question.Order)
            .ExecuteUpdateAsync(x => x.SetProperty(prop => prop.Order, prop => prop.Order - 1), cancellationToken);
        // If the user has confirmed to delete. Hard delete the item
        var deletedCount = await _dbContext.ModelExamQuestionConfigurations
            .Where(x => x.Id == request.ModelExamQuestionId)
            .ExecuteDeleteAsync(cancellationToken);
        return new(true);
    }
}