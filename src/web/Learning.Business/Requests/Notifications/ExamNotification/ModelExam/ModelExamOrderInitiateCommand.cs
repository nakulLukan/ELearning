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
    public int ModelExamId { get; set; }
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
        var examPackage = await (from mec in _dbContext.ModelExamConfigurations
                                 join mep in _dbContext.ModelExamPackages
                                 on mec.ModelExamPackageId equals mep.Id
                                 where mec.Id == request.ModelExamId
                                 select mep).FirstAsync(cancellationToken).ConfigureAwait(false);
        var userId = await _requestContext.GetUserId().ConfigureAwait(false);
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
        return new(initiateOrder.Id);
    }
}
