using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam;

public class ModelExamOrderCompleteCommand : IRequest<ModeExamOrderCompleteResponseDto>
{
    public required long ModelExamOrderId { get; set; }
    public required OrderStatusEnum OrderStatus { get; set; }
}

public class ModelExamOrderCompleteCommandHandler : IRequestHandler<ModelExamOrderCompleteCommand, ModeExamOrderCompleteResponseDto>
{
    private readonly IAppDbContext _dbContext;

    public ModelExamOrderCompleteCommandHandler(
        IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ModeExamOrderCompleteResponseDto> Handle(ModelExamOrderCompleteCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.ModelExamPurchaseHistory.AnyAsync(x => x.OrderId == request.ModelExamOrderId, cancellationToken))
        {
            throw new AppException("Order already confirmed");
        }

        var order = await _dbContext.ModelExamOrders.FirstAsync(x => x.Id == request.ModelExamOrderId).ConfigureAwait(false);
        // TODO: Check if the payment is compelted in razorpay
        order.Status = OrderStatusEnum.Completed;
        order.OrderedCompletedOn = AppDateTime.UtcNow;

        // Create purchase history object
        ModelExamPurchaseHistory purchaseHistory = new ModelExamPurchaseHistory
        {
            OrderId = order.Id,
            PurchasedOn = order.OrderedCompletedOn.Value,
            ValidTill = AppDateTime.UtcNow.AddYears(1),
        };

        _dbContext.ModelExamPurchaseHistory.Add(purchaseHistory);
        await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
        return new()
        {
            Amount = order.Amount,
            CompletedOn = order.OrderedCompletedOn.Value,
            OrderId = order.Id,
            Status = order.Status,
        };
    }
}
