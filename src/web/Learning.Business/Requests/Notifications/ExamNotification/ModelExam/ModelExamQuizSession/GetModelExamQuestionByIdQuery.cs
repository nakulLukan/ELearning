using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.ModelExamQuizSession;

public class GetModelExamQuestionByIdQuery : IRequest<ModelExamSessionQuestionDetailDto>
{
    public required int QuestionId { get; set; }
    public required long ModelExamResultId { get; set; }
}
public class GetModelExamQuestionByIdQueryHandler : IRequestHandler<GetModelExamQuestionByIdQuery, ModelExamSessionQuestionDetailDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IApiRequestContext _requestContext;

    public GetModelExamQuestionByIdQueryHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<ModelExamSessionQuestionDetailDto> Handle(GetModelExamQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.ModelExamQuestionConfigurations
            .Where(x => x.Id == request.QuestionId)
            .Select(x => new
            {
                QuestionId = request.QuestionId,
                ModelExamId = x.ExamConfigId,
                QuestionText = x.QuestionText!,
                QuestionImageRelativePath = x.QuestionImage != null ? x.QuestionImage.RelativePath : null,
                Options = x.ModelExamAnswers!.OrderBy(y => y.Order).Select(y => new
                {
                    AnswerId = y.Id,
                    AnswerText = y.AnswerText,
                    AnswerImageRelativePath = y.AnswerImage != null ? y.AnswerImage.RelativePath : null
                }).ToArray()
            })
            .FirstAsync(cancellationToken);

        var userId = await _requestContext.GetUserId();
        //var hasSubscription = await (from mep in _dbContext.ModelExamPackages
        //                             join mec in _dbContext.ModelExamConfigurations
        //                             on mep.Id equals mec.ModelExamPackageId
        //                             join meo in _dbContext.ModelExamOrders
        //                             on mep.Id equals meo.ModelExamPackageId
        //                             join meph in _dbContext.ModelExamPurchaseHistory
        //                             on mep.Id equals meph.OrderId
        //                             where mec.Id == question.ModelExamId && meo.Status == Shared.Common.Enums.OrderStatusEnum.Success && meph.ValidTill > AppDateTime.UtcNow && meo.UserId == userId
        //                             select meo.Id
        //                             ).AnyAsync(cancellationToken);

        //if (!hasSubscription)
        //{
        //}

        // It is assumed that the user has valid session/subscription if the exam result id is valid
        var selectedOptionDetail = await (from mer in _dbContext.ModelExamResults
                                          join merd in _dbContext.ModelExamResultDetails
                                              on mer.Id equals merd.ModelExamResultId into merdGroup
                                          from merd in merdGroup.Where(x=>x.QuestionId == request.QuestionId).DefaultIfEmpty() // Left join
                                          where mer.Id == request.ModelExamResultId && mer.UserId == userId 
                                          select new
                                          {
                                              ModelExamResultId = mer.Id,
                                              SelectedAnswerId = merd != null ? merd.SelectedAnswerId : null
                                          }).SingleOrDefaultAsync(cancellationToken);
        if (selectedOptionDetail == null)
        {
            throw new AppException("Please purchase the model exam package");
        }

        return new ModelExamSessionQuestionDetailDto()
        {
            QuestionId = question.QuestionId,
            QuestionImageUrl = question.QuestionImageRelativePath != null ? _fileStorage.GetPresignedUrl(question.QuestionImageRelativePath) : null,
            QuestionText = question.QuestionText,
            SelectedOptionId = selectedOptionDetail.SelectedAnswerId,
            Options = question.Options.Select((x, index) => new ModelExamSessionOptionDetailDto()
            {
                AnswerId = x.AnswerId,
                OptionText = x.AnswerText,
                OptionImageUrl = x.AnswerImageRelativePath != null ? _fileStorage.GetPresignedUrl(x.AnswerImageRelativePath) : null,
                Order = index
            }).ToArray(),
        };
    }
}

