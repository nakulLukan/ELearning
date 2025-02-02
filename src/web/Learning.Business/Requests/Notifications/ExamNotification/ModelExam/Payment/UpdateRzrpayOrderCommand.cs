using Learning.Business.Contracts.HttpContext;
using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Payment;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Payment;

public class UpdateRzrpayOrderCommand : IRequest<ModelExamOrderStepDetailDto>
{
    public required long ModelExamOrderId { get; set; }
    public required string RazorpayOrderId { get; set; }
}

public class UpdateRzrpayOrderCommandHandler : IRequestHandler<UpdateRzrpayOrderCommand, ModelExamOrderStepDetailDto>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;

    public UpdateRzrpayOrderCommandHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ModelExamOrderStepDetailDto> Handle(UpdateRzrpayOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var order = await _appDbContext.ModelExamOrders
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId, cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEO001", "Order not found");
        order.RzrpayOrderId = request.RazorpayOrderId;
        order.Status = Shared.Common.Enums.OrderStatusEnum.RzrpayOrder;
        await _appDbContext.SaveAsync(cancellationToken);
        return new ModelExamOrderStepDetailDto
        {
            Amount = order.Amount,
            ModelExamOrderId = order.Id,
            RazorpayOrderId = request.RazorpayOrderId,
            Status = order.Status
        };
    }
}
