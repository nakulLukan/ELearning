using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;

public class DeleteFailedOrderCommand : IRequest<ResponseDto<bool>>
{
    public required long ModelExamOrderId { get; set; }
}

public class DeleteFailedOrderCommandHandler : IRequestHandler<DeleteFailedOrderCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;

    public DeleteFailedOrderCommandHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<bool>> Handle(DeleteFailedOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var count = await _appDbContext.ModelExamOrders
            .Where(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId
                && x.Status == Shared.Common.Enums.OrderStatusEnum.Failed)
            .ExecuteDeleteAsync(cancellationToken);

        return new(count > 0);
    }
}
