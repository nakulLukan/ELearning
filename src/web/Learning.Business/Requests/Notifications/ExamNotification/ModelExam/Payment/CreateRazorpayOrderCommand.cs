using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.PaymentGateway;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.ModelExam.Payment;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;

public class CreateRazorpayOrderCommand : IRequest<ModelExamOrderStepDetailDto>
{
    public required long ModelExamOrderId { get; set; }
}

public class CreateRazorpayOrderCommandHandler : IRequestHandler<CreateRazorpayOrderCommand, ModelExamOrderStepDetailDto>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;
    private readonly IAppPaymentGateway _paymentGateway;
    private readonly IConfiguration _configuration;

    public CreateRazorpayOrderCommandHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext,
        IAppPaymentGateway paymentGateway,
        IConfiguration configuration)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
        _paymentGateway = paymentGateway;
        _configuration = configuration;
    }

    public async Task<ModelExamOrderStepDetailDto> Handle(CreateRazorpayOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var emailId = await _requestContext.GetEmail();
        var phoneNumber = await _requestContext.GetPhoneNumber();
        var name = await _requestContext.GetName();

        var orderDetails = await _appDbContext.ModelExamOrders
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ModelExamOrderId && x.UserId == userId, cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.BadRequest, "CRP001", "Order not found");

        var amountInPaisa = _paymentGateway.ConvertInrToPaisa(orderDetails.Amount);
        var rzrpayOrderId = _paymentGateway.CreateOrder(request.ModelExamOrderId.ToString(), amountInPaisa);
        orderDetails.RzrpayOrderId = rzrpayOrderId;
        orderDetails.Status = Shared.Common.Enums.OrderStatusEnum.RzrpayOrderCreated;
        await _appDbContext.SaveAsync(cancellationToken);
        return new ModelExamOrderStepDetailDto
        {
            AmountInPaisa = amountInPaisa,
            ModelExamOrderId = orderDetails.Id,
            RazorpayOrderId = rzrpayOrderId,
            RazorpayApiKey = _configuration[AppSettingsKeyConstant.PaymentGateway_AccessKey]!,
            Status = orderDetails.Status,
            Email = emailId,
            Name = name,
            PhoneNumber = phoneNumber,
            NotificationTitle = null,
            OrderCompletedOn = null,
            Validity = null,
            TotalPaidExamsInPackage = 0,
            ModelExamId = 0,
            ExamNotificationId = 0,
            ModelExamOrderReferenceId = string.Empty
        };
    }
}
