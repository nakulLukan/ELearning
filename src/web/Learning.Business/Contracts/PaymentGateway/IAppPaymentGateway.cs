using Learning.Shared.Enums;

namespace Learning.Business.Contracts.PaymentGateway;

public interface IAppPaymentGateway
{
    public string CreateOrder(string internalOrderId, double amountInPaisa);
    public double ConvertInrToPaisa(float rupees);
    public PaymentGatewayOrderStatusEnum GetOrderStatus(string rzrpayOrderId);
}
