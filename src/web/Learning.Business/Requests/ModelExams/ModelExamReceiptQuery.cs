using Learning.Business.Constants;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.PaymentGateway;
using Learning.Business.Impl.Data;
using Learning.Business.Services.ExamNotification;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExams.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.ModelExams;

public class ModelExamReceiptQuery : IRequest<ModelExamPaymentReceipt>
{
    public required long ModelExamOrderId { get; set; }
}

public class ModelExamReceiptQueryHandler : IRequestHandler<ModelExamReceiptQuery, ModelExamPaymentReceipt>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IAppPaymentGateway _paymentGateWay;
    private readonly IApiRequestContext _requestContext;
    public ModelExamReceiptQueryHandler(
        IAppDbContextFactory appDbContext, IAppPaymentGateway paymentGateWay, IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _paymentGateWay = paymentGateWay;
        _requestContext = requestContext;
    }

    public async Task<ModelExamPaymentReceipt> Handle(ModelExamReceiptQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var order = await _appDbContext.ModelExamOrders.IgnoreQueryFilters()
            .Where(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId
                && x.Status == Shared.Common.Enums.OrderStatusEnum.Success)
            .Select(x => new
            {
                x.Amount,
                x.User!.OtherDetails!.FullName,
                x.OrderedCompletedOn,
                x.ModelExamPackage!.ExamNotification!.NotificationTitle,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "PR001", "Receipt not found");
        var itemPrice = order.Amount / (1 + PaymentConstant.GstPercentage);
        return new ModelExamPaymentReceipt
        {
            GstAmount = itemPrice * PaymentConstant.GstPercentage,
            Gst = (int)(PaymentConstant.GstPercentage * 100),
            Item = order.NotificationTitle,
            ItemAmount = itemPrice,
            OrderReferenceId = ExamNotificationHelper.GetModelExamOrderReferenceId(request.ModelExamOrderId),
            OrderPurchasedOn = order.OrderedCompletedOn!.Value,
            SoldTo = order.FullName!,
            TotalPaid = order.Amount,
        };
    }
}

