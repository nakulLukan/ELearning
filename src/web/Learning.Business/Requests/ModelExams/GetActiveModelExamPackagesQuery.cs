using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExams;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.ModelExams;

public class GetActiveModelExamPackagesQuery : IRequest<ActiveModelExamPackageBasicDetailDto[]>
{
}

public class GetActiveModelExamPackagesQueryHandler : IRequestHandler<GetActiveModelExamPackagesQuery, ActiveModelExamPackageBasicDetailDto[]>
{
    private readonly IExamNotificationManager _notificationManager;
    private readonly IApiRequestContext _requestContext;
    private readonly IAppDbContext _appDbContext;
    private readonly IFileStorage _fileStorage;
    public GetActiveModelExamPackagesQueryHandler(
        IExamNotificationManager notificationManager,
        IApiRequestContext requestContext,
        IAppDbContextFactory appDbContext,
        IFileStorage fileStorage)
    {
        _notificationManager = notificationManager;
        _requestContext = requestContext;
        _appDbContext = appDbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ActiveModelExamPackageBasicDetailDto[]> Handle(GetActiveModelExamPackagesQuery request, CancellationToken cancellationToken)
    {
        var examNotifications = await _notificationManager.GetAllActiveExamNotifications(cancellationToken);
        string? userId = null;
        if (await _requestContext.IsAuthenticated())
        {
            userId = await _requestContext.GetUserId();
        }

        List<ActiveModelExamPackageBasicDetailDto> purchasedPackages = [];
        if (!string.IsNullOrEmpty(userId))
        {
            purchasedPackages = await _appDbContext.ModelExamPurchaseHistory.IgnoreQueryFilters()
               .Where(x => x.ModelExamOrder!.UserId == userId
                   && x.ValidTill >= AppDateTime.UtcNow
                   && x.ModelExamOrder!.ModelExamPackage!.ExamNotification!.IsActive)
               .Select(x => new ActiveModelExamPackageBasicDetailDto
               {
                   ExamNotificationId = x.ModelExamOrder!.ModelExamPackage!.ExamNotificationId,
                   ExamNotificationName = x.ModelExamOrder.ModelExamPackage.ExamNotification!.NotificationTitle,
                   ImageAbsUrl = _fileStorage.GetObjectUrl(x.ModelExamOrder.ModelExamPackage.ExamNotification.ImageRelativePath),
                   IsPurchased = true,
               })
               .ToListAsync(cancellationToken);
        }


        var activeExamNotifications = examNotifications
            .Select(x => new ActiveModelExamPackageBasicDetailDto
            {
                ExamNotificationId = x.NotificationId,
                ExamNotificationName = x.Title,
                IsPurchased = false,
                ImageAbsUrl = x.ImageRelativePath
            }).ToArray();
        return activeExamNotifications
            .UnionBy(purchasedPackages, x => x.ExamNotificationId)
            .Select(x => new ActiveModelExamPackageBasicDetailDto()
            {
                ExamNotificationId = x.ExamNotificationId,
                ExamNotificationName = x.ExamNotificationName,
                ImageAbsUrl = x.ImageAbsUrl,
                IsPurchased = purchasedPackages.Any(y => y.ExamNotificationId == x.ExamNotificationId),
            })
            .ToArray();
    }
}

