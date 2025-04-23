using Learning.Business.Dto.PurchaseHistory;
using Learning.Business.Impl.Data;
using Learning.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.PurchaseHistory;

public class AdminOrderHistoryQuery : IRequest<PaginatedResponse<AdminOrderHistoryItemDto>>
{
    public int Skip { get; set; }
    public int Take { get; set; }
}
public class AdminOrderHistoryQueryHandler : IRequestHandler<AdminOrderHistoryQuery, PaginatedResponse<AdminOrderHistoryItemDto>>
{
    private readonly IAppDbContext _dbContext;

    public AdminOrderHistoryQueryHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<PaginatedResponse<AdminOrderHistoryItemDto>> Handle(AdminOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        var count = await _dbContext.ModelExamOrders.CountAsync(cancellationToken);
        var data = await _dbContext.ModelExamOrders.IgnoreQueryFilters()
            .OrderByDescending(x => x.Id)
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new AdminOrderHistoryItemDto()
            {
                Amount = x.Amount,
                PurchasedOn = x.ModelExamPurchaseHistory != null ? x.ModelExamPurchaseHistory.PurchasedOn : null,
                ExamNotificationTitle = x.ModelExamPackage!.ExamNotification!.NotificationTitle,
                InitiatedOn = x.OrderedInitiatedOn,
                OrderId = x.Id,
                CompletedOn = x.OrderedCompletedOn,
                RzrPayOrderId = x.RzrpayOrderId,
                Status = x.Status,
                Fullname = x.User!.OtherDetails!.FullName ?? "-",
                ValidTill = x.ModelExamPurchaseHistory != null ? x.ModelExamPurchaseHistory.ValidTill : null,
            })
            .ToArrayAsync(cancellationToken);

        return new(data, count);
    }
}
