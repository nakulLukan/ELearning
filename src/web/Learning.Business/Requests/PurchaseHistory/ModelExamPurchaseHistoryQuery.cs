using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Dto.PurchaseHistory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.PurchaseHistory;

public class ModelExamPurchaseHistoryQuery : IRequest<ModelExamPurchaseHistoryItemDto[]>
{
}
public class ModelExamPurchaseHistoryQueryHandler : IRequestHandler<ModelExamPurchaseHistoryQuery, ModelExamPurchaseHistoryItemDto[]>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;

    public ModelExamPurchaseHistoryQueryHandler(IAppDbContextFactory appDbContext, IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ModelExamPurchaseHistoryItemDto[]> Handle(ModelExamPurchaseHistoryQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var orders = await _appDbContext.ModelExamOrders.IgnoreQueryFilters()
            .Where(x => x.UserId == userId
                && x.Status == Shared.Common.Enums.OrderStatusEnum.Success)
            .OrderByDescending(x=>x.OrderedCompletedOn)
            .Select(x => new ModelExamPurchaseHistoryItemDto
            {
                TotalPrice = x.Amount,
                PurchasedOn = x.OrderedCompletedOn!.Value,
                ExamNotificationName = x.ModelExamPackage!.ExamNotification!.NotificationTitle,
                ModelExamOrderId = x.Id,
                ExamNotificationId = x.ModelExamPackage.ExamNotificationId
            })
            .ToArrayAsync(cancellationToken);
        return orders;
    }
}

