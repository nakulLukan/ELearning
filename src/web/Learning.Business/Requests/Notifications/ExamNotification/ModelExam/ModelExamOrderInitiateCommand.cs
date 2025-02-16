using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam;

public class ModelExamOrderInitiateCommand : IRequest<ResponseDto<long>>
{
    public required int ExamNotificationId { get; set; }
}
public class ModelExamOrderInitiateCommandHandler : IRequestHandler<ModelExamOrderInitiateCommand, ResponseDto<long>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IApiRequestContext _requestContext;

    public ModelExamOrderInitiateCommandHandler(
        IAppDbContextFactory dbContext,
        IApiRequestContext requestContext)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<long>> Handle(ModelExamOrderInitiateCommand request, CancellationToken cancellationToken)
    {
        var examPackage = await _dbContext.ModelExamPackages
            .Where(x => x.ExamNotificationId == request.ExamNotificationId)
            .Select(x => new
            {
                x.DiscountedPrice,
                x.Id
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEOI001", "Unknown model exam package");
        var userId = await _requestContext.GetUserId().ConfigureAwait(false);

        // If there is any existing incomplete order for this exam notification then use that.
        var existingPendingOrderId = await _dbContext.ModelExamOrders
            .Where(x => x.UserId == userId
                && (x.Status == Shared.Common.Enums.OrderStatusEnum.Initiated
                   || x.Status == Shared.Common.Enums.OrderStatusEnum.RzrpayOrderCreated)
                && x.ModelExamPackage!.ExamNotificationId == request.ExamNotificationId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (existingPendingOrderId == default)
        {
            ModelExamOrder initiateOrder = new ModelExamOrder
            {
                Id = 0,
                Amount = examPackage.DiscountedPrice,
                ModelExamPackageId = examPackage.Id,
                OrderedInitiatedOn = AppDateTime.UtcNow,
                Status = Shared.Common.Enums.OrderStatusEnum.Initiated,
                UserId = userId
            };

            _dbContext.ModelExamOrders.Add(initiateOrder);
            await _dbContext.SaveAsync(cancellationToken).ConfigureAwait(false);
            existingPendingOrderId = initiateOrder.Id;
        }
        return new(existingPendingOrderId);
    }
}
