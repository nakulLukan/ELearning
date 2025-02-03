using Learning.Business.Contracts.PaymentGateway;
using Learning.Shared.Common.Constants;
using Learning.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Razorpay.Api;

namespace Learning.Infrastructure.Impl.PaymentGateway;

public class RazorpayPaymentGateway : IAppPaymentGateway
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RazorpayPaymentGateway> _logger;

    public RazorpayPaymentGateway(IConfiguration configuration, ILogger<RazorpayPaymentGateway> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string CreateOrder(string internalOrderId, double amountInPaisa)
    {
        var client = new RazorpayClient(_configuration[AppSettingsKeyConstant.PaymentGateway_AccessKey], _configuration[AppSettingsKeyConstant.PaymentGateway_SecretKey]);
        Dictionary<string, object> options = new()
        {
            { "amount", amountInPaisa }, // Convert amount to paise
            { "currency", "INR" },
            { "receipt", internalOrderId },
            { "payment_capture", 1 } // Auto-capture
        };

        Order order = client.Order.Create(options);
        return order["id"].ToString();
    }

    public double ConvertInrToPaisa(float rupees)
    {
        return Math.Round(rupees * 100, 2);
    }

    public PaymentGatewayOrderStatusEnum GetOrderStatus(string rzrpayOrderId)
    {
        try
        {
            var client = new RazorpayClient(_configuration[AppSettingsKeyConstant.PaymentGateway_AccessKey], _configuration[AppSettingsKeyConstant.PaymentGateway_SecretKey]);
            var orderDetails = client.Order.Fetch(rzrpayOrderId);
            var status = Convert.ToString(orderDetails.Attributes.status);
            if (status == "paid")
            {
                return PaymentGatewayOrderStatusEnum.Paid;
            }
            else if (status == "attempted" )
            {
                return PaymentGatewayOrderStatusEnum.Attempted;
            }
            else
            {
                return PaymentGatewayOrderStatusEnum.Created;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Order status failed. {OrderId}", rzrpayOrderId);
            return PaymentGatewayOrderStatusEnum.Invalid;
        }
    }
}
