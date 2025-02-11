using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExams;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.ModelExams;

public class ModelExamPurchaseNowQuery : IRequest<ModelExamPurchaseNowDto>
{
    public required int ExamNotificationId { get; set; }
}

public class ModelExamPurchaseNowQueryHandler : IRequestHandler<ModelExamPurchaseNowQuery, ModelExamPurchaseNowDto>
{
    private readonly IAppDbContext _appDbContext;
    public ModelExamPurchaseNowQueryHandler(
        IAppDbContextFactory appDbContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
    }

    public async Task<ModelExamPurchaseNowDto> Handle(ModelExamPurchaseNowQuery request, CancellationToken cancellationToken)
    {
        var examPackageDetails = await _appDbContext.ModelExamPackages.IgnoreQueryFilters()
            .Where(x => x.ExamNotificationId == request.ExamNotificationId)
            .Select(x => new ModelExamPurchaseNowDto()
            {
                DiscountedPrice = x.DiscountedPrice,
                ExamNotificationDescription = x.ExamNotification!.Description,
                ExamNotificationId = request.ExamNotificationId,
                ExamNotificationName = x.ExamNotification.NotificationTitle,
                Price = x.Price,
                ValidUpto = DateOnly.FromDateTime(AppDateTime.UtcNow.AddYears(1).DateTime)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEPN001", "Uknown package");

        return examPackageDetails;
    }
}

