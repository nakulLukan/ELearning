using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.PurchaseHistory;

public class AdminOrderHistoryDeleteCommand : IRequest<ResponseDto<bool>>
{
    public required long OrderId { get; set; }
}
public class AdminOrderHistoryDeleteCommandHandler : IRequestHandler<AdminOrderHistoryDeleteCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;

    public AdminOrderHistoryDeleteCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ResponseDto<bool>> Handle(AdminOrderHistoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var count = await _dbContext.ModelExamOrders
            .Where(x => x.Id == request.OrderId
                && x.Status != Shared.Common.Enums.OrderStatusEnum.Success)
            .ExecuteDeleteAsync(cancellationToken);
        if (count > 0)
        {
            return new ResponseDto<bool>(true);
        }

        return new ResponseDto<bool>(false);
    }
}
