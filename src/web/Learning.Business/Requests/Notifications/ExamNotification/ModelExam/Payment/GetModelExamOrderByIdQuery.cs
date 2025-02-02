using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.PaymentGateway;
using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Payment;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;

public class GetModelExamOrderByIdQuery : IRequest<ModelExamOrderStepDetailDto>
{
    public long ModelExamOrderId { get; set; }
}

public class GetModelExamOrderByIdQueryHandler : IRequestHandler<GetModelExamOrderByIdQuery, ModelExamOrderStepDetailDto>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly IAppPaymentGateway _paymentGateway;

    public GetModelExamOrderByIdQueryHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext,
        IAppPaymentGateway paymentGateway)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
        _paymentGateway = paymentGateway;
    }

    public async Task<ModelExamOrderStepDetailDto> Handle(GetModelExamOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var order = await _appDbContext.ModelExamOrders
            .FirstOrDefaultAsync(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId, cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEO001", "Order not found");

        if (order.Status == Shared.Common.Enums.OrderStatusEnum.RzrpayOrderCreated && !string.IsNullOrEmpty(order.RzrpayOrderId))
        {
            var orderStatus = _paymentGateway.GetOrderStatus(order.RzrpayOrderId!);
            switch (orderStatus)
            {
                case Shared.Enums.PaymentGatewayOrderStatusEnum.Paid:
                    order.Status = Shared.Common.Enums.OrderStatusEnum.Success;
                    order.OrderedCompletedOn = AppDateTime.UtcNow;
                    break;
                case Shared.Enums.PaymentGatewayOrderStatusEnum.Attempted:
                    order.Status = Shared.Common.Enums.OrderStatusEnum.Failed;
                    order.OrderedCompletedOn = AppDateTime.UtcNow;
                    break;
                default: break;
            }

            await _appDbContext.ModelExamOrders.Where(x => x.Id == request.ModelExamOrderId)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(prop => prop.Status, order.Status)
                    .SetProperty(prop => prop.OrderedCompletedOn, order.OrderedCompletedOn), cancellationToken);
        }
        return new ModelExamOrderStepDetailDto
        {
            AmountInPaisa = 0,
            ModelExamOrderId = request.ModelExamOrderId,
            RazorpayOrderId = order.RzrpayOrderId,
            Status = order.Status,
            Email = null,
            Name = null,
            PhoneNumber = null
        };
    }
}
