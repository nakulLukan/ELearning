using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.PaymentGateway;
using Learning.Business.Impl.Data;
using Learning.Business.Services.ExamNotification;
using Learning.Domain.Notification;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExam.Payment;
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

    public class OrderQueryDto
    {
        public required long OrderId { get; set; }
        public required OrderStatusEnum Status { get; set; }
        public required float Amount { get; set; }
        public required string? RzrpayOrderId { get; set; }
        public required DateTimeOffset? OrderCompletedOn { get; set; }
        public required string ExamNotificationTitle { get; set; }
        public required DateTimeOffset? Validity { get; set; }
        public required int TotalExamsInPackage { get; set; }
        public required int ModelExamId { get; set; }
        public required int ExamNotificationId { get; set; }
    }

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
            .Where(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId)
            .Select(x => new OrderQueryDto
            {
                OrderId = x.Id,
                Status = x.Status,
                Amount = x.Amount,
                RzrpayOrderId = x.RzrpayOrderId,
                OrderCompletedOn = x.OrderedCompletedOn,
                ExamNotificationTitle = x.ModelExamPackage!.ExamNotification!.NotificationTitle,
                ExamNotificationId = x.ModelExamPackage.ExamNotificationId,
                Validity = x.ModelExamPurchaseHistory != null ? x.ModelExamPurchaseHistory!.ValidTill : null,
                TotalExamsInPackage = x.ModelExamPackage.ModelExamConfigs!.Where(x => !x.IsFree).Count(),
                ModelExamId = x.ModelExamPackage.ModelExamConfigs!.First(y => !y.IsFree).Id
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEO001", "Order not found");

        if (order.Status == OrderStatusEnum.RzrpayOrderCreated && !string.IsNullOrEmpty(order.RzrpayOrderId))
        {
            var orderStatus = _paymentGateway.GetOrderStatus(order.RzrpayOrderId!);
            switch (orderStatus)
            {
                case Shared.Enums.PaymentGatewayOrderStatusEnum.Paid:
                    order.Status = Shared.Common.Enums.OrderStatusEnum.Success;
                    order.OrderCompletedOn = AppDateTime.UtcNow;
                    break;
                case Shared.Enums.PaymentGatewayOrderStatusEnum.Attempted:
                    order.Status = Shared.Common.Enums.OrderStatusEnum.Failed;
                    order.OrderCompletedOn = AppDateTime.UtcNow;
                    break;
                default: break;
            }
            await CreateModelExamPurchaseHistory(request, order, cancellationToken).ConfigureAwait(false);
        }
        return new ModelExamOrderStepDetailDto
        {
            AmountInPaisa = 0,
            ModelExamOrderId = request.ModelExamOrderId,
            RazorpayOrderId = order.RzrpayOrderId,
            Status = order.Status,
            Email = null,
            Name = null,
            PhoneNumber = null,
            NotificationTitle = order.ExamNotificationTitle,
            OrderCompletedOn = order.OrderCompletedOn,
            Validity = order.Validity,
            TotalPaidExamsInPackage = order.TotalExamsInPackage,
            ModelExamId = order.ModelExamId,
            ExamNotificationId = order.ExamNotificationId,
            ModelExamOrderReferenceId = ExamNotificationHelper.GetModelExamOrderReferenceId(request.ModelExamOrderId)
        };
    }

    private async Task CreateModelExamPurchaseHistory(GetModelExamOrderByIdQuery request, OrderQueryDto order, CancellationToken cancellationToken)
    {
        if (order.Status == Shared.Common.Enums.OrderStatusEnum.Success || order.Status == Shared.Common.Enums.OrderStatusEnum.Failed)
        {
            await _appDbContext.ModelExamOrders.Where(x => x.Id == request.ModelExamOrderId)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(prop => prop.Status, order.Status)
                    .SetProperty(prop => prop.OrderedCompletedOn, order.OrderCompletedOn), cancellationToken);

            // Create purchase history object
            ModelExamPurchaseHistory purchaseHistory = new ModelExamPurchaseHistory
            {
                OrderId = order.OrderId,
                PurchasedOn = order.OrderCompletedOn!.Value,
                ValidTill = AppDateTime.UtcNow.AddYears(1),
            };

            _appDbContext.ModelExamPurchaseHistory.Add(purchaseHistory);
            await _appDbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
            order.Validity = purchaseHistory.ValidTill;
        }
    }
}
