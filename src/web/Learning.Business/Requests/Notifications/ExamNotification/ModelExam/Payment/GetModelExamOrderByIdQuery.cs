using Learning.Business.Contracts.HttpContext;
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

    public GetModelExamOrderByIdQueryHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ModelExamOrderStepDetailDto> Handle(GetModelExamOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var order = await _appDbContext.ModelExamOrders
            .FirstOrDefaultAsync(x => x.Id == request.ModelExamOrderId
                && x.UserId == userId, cancellationToken) ?? throw new AppApiException(System.Net.HttpStatusCode.NotFound, "MEO001", "Order not found");
        return new ModelExamOrderStepDetailDto
        {
            Amount = order.Amount,
            ModelExamOrderId = request.ModelExamOrderId,
            RazorpayOrderId = order.RzrpayOrderId,
            Status = order.Status,
        };
    }
}
