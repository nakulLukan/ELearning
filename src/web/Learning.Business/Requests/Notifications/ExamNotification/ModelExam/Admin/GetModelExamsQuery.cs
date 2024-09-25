using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class GetModelExamsQuery : IRequest<ModelExamConfigurationResponseDto[]>
{
    public int ExamNotificationId { get; set; }
}

public class GetModelExamsQueryHandler : IRequestHandler<GetModelExamsQuery, ModelExamConfigurationResponseDto[]>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IFileStorage _fileStorage;

    public GetModelExamsQueryHandler(
        IAppDbContextFactory appDbContext,
        IFileStorage fileStorage)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ModelExamConfigurationResponseDto[]> Handle(GetModelExamsQuery request, CancellationToken cancellationToken)
    {
        var modelExams = await _appDbContext.ModelExamConfigurations
            .Where(x => x.ExamNotificationId == request.ExamNotificationId)
            .OrderBy(x => x.CreatedOn)
            .Select(x => new ModelExamConfigurationResponseDto
            {
                Id = x.Id,
                CreatedOn = x.CreatedOn!.Value,
                DiscountedPrice = x.ModelExamPackage!.DiscountedPrice,
                ExamDescription = x.Description,
                ExamName = x.ExamName,
                IsActive = x.IsActive,
                IsFree = x.IsFree,
                Price = x.ModelExamPackage!.Price,
                SolutionVideoSignedUrl = _fileStorage.GetPresignedUrl(StoragePathConstant.ExamNotificationModelExamBasePath(x.Id, request.ExamNotificationId)),
                ExamNotificationId = x.ExamNotificationId,
                TotalQuestions = x.Questions!.Count(),
                TotalTimeLimitInSeconds = x.TotalTimeLimit,
            })
            .ToArrayAsync(cancellationToken);

        return modelExams;
    }
}
