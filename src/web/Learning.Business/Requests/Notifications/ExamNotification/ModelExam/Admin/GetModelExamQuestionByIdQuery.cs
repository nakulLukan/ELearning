using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class GetModelExamQuestionByIdQuery : IRequest<ModelExamQuestionDetailResponseDto>
{
    public required int ModelExamId { get; set; }
    public required int QuestionId { get; set; }
}

public class GetModelExamQuestionByIdQueryHandler : IRequestHandler<GetModelExamQuestionByIdQuery, ModelExamQuestionDetailResponseDto>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IFileStorage _fileStorage;

    public GetModelExamQuestionByIdQueryHandler(
        IAppDbContextFactory appDbContext,
        IFileStorage fileStorage)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ModelExamQuestionDetailResponseDto> Handle(GetModelExamQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var questionDetail = await _appDbContext.ModelExamQuestionConfigurations
            .Where(x => x.ExamConfigId == request.ModelExamId && x.Id == request.QuestionId)
            .Select(x => new ModelExamQuestionDetailResponseDto
            {
                Id = x.Id,
                IsActive = x.IsActive,
                Order = x.Order,
                QuestionText = x.QuestionText,
                QuestionImageSignedUrl = x.QuestionImage != null ? _fileStorage.GetPresignedUrl(x.QuestionImage.RelativePath) : null,
                Options = x.ModelExamAnswers!.Where(y => y.QuestionId == request.QuestionId).Select(y => new ModelExamOptionDetailResponseDto
                {
                    Id = y.Id,
                    IsCorrectOption = y.IsCorrectAnswer,
                    Order = y.Order,
                    QuestionId = y.QuestionId,
                    OptionText = y.AnswerText,
                    OptionImageSignedUrl = y.AnswerImage != null ? _fileStorage.GetPresignedUrl(y.AnswerImage.RelativePath) : null,
                }).ToArray()
            })
            .FirstAsync(cancellationToken);

        return questionDetail;
    }
}
