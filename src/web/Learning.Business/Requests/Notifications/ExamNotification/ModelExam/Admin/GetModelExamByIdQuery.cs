using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class GetModelExamByIdQuery : IRequest<ModelExamDetailResponseDto>
{
    public int ExamNotificationId { get; set; }
    public int ModelExamId { get; set; }
}

public class GetModelExamByIdQueryHandler : IRequestHandler<GetModelExamByIdQuery, ModelExamDetailResponseDto>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IFileStorage _fileStorage;

    public GetModelExamByIdQueryHandler(
        IAppDbContextFactory appDbContext,
        IFileStorage fileStorage)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ModelExamDetailResponseDto> Handle(GetModelExamByIdQuery request, CancellationToken cancellationToken)
    {
        var modelExamDetail = await _appDbContext.ModelExamConfigurations
            .Where(x => x.Id == request.ModelExamId)
            .Select(x => new ModelExamDetailResponseDto
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn!.Value,
                DiscountedPrice = x.ModelExamPackage!.DiscountedPrice,
                ExamDescription = x.Description,
                ExamName = x.ExamName,
                IsActive = x.IsActive,
                IsFree = x.IsFree,
                Price = x.ModelExamPackage!.Price,
                SolutionVideoSignedUrl = x.ExamSolutionVideo != null ? _fileStorage.GetPresignedUrl(x.ExamSolutionVideo.RelativePath) : null,
                ExamNotificationId = x.ExamNotificationId,
                TotalTimeLimitInSeconds = x.TotalTimeLimit,
                Questions = new ModelExamQuestionMetaData[0],
                SolutionVideoMpdFileName = x.ExamSolutionVideo != null ? x.ExamSolutionVideo.FileName : null,
                SolutionVideoUploadLink = _fileStorage.GetS3ConsoleLink(StoragePathConstant.ModelExamSolutionVideoBasPath(request.ExamNotificationId, x.Id))
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Unknwon exam notification", true);

        var questions = await _appDbContext.ModelExamQuestionConfigurations
            .Where(x => x.ExamConfigId == request.ModelExamId)
            .OrderBy(x => x.Order)
            .Select(x => new ModelExamQuestionMetaData
            {
                Id = x.Id,
                Order = x.Order,
                IsActive = x.IsActive,
                Score = x.Score,
                QuestionText = x.QuestionText,
                QuestionImageUrl = x.QuestionImage != null ? _fileStorage.GetPresignedUrl(x.QuestionImage.RelativePath) : null,
                OptionNumber = x.ModelExamAnswers!.First(x => x.IsCorrectAnswer).Order.ToString()
            })
            .ToArrayAsync(cancellationToken);
        modelExamDetail.Questions = questions;
        return modelExamDetail;
    }
}
