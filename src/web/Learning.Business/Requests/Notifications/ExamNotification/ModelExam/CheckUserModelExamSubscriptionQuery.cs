using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam;

public class CheckUserModelExamSubscriptionQuery : IRequest<ResponseDto<bool>>
{
    public int ModelExamId { get; set; }
}

public class CheckUserModelExamSubscriptionQueryHandler : IRequestHandler<CheckUserModelExamSubscriptionQuery, ResponseDto<bool>>
{
    private readonly IAppDbContext _appDbContext;
    private readonly IApiRequestContext _requestContext;

    public CheckUserModelExamSubscriptionQueryHandler(
        IAppDbContextFactory appDbContext,
        IApiRequestContext requestContext)
    {
        _appDbContext = appDbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ResponseDto<bool>> Handle(CheckUserModelExamSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var result = await (from ec in _appDbContext.ModelExamConfigurations.IgnoreQueryFilters()
                            join ep in _appDbContext.ModelExamPackages on ec.ModelExamPackageId equals ep.Id
                            join epo in _appDbContext.ModelExamOrders on ep.Id equals epo.ModelExamPackageId
                            join eph in _appDbContext.ModelExamPurchaseHistory on epo.Id equals eph.OrderId
                            where ec.Id == request.ModelExamId
                               && epo.UserId == userId
                               && eph.ValidTill >= DateTime.UtcNow.Date
                            select new
                            {
                                PurchaseId = eph.Id
                            }).FirstOrDefaultAsync(cancellationToken);

        var hasValidSubscription = result != null;
        if (!hasValidSubscription)
        {
            hasValidSubscription = await _appDbContext.ModelExamConfigurations.AnyAsync(x => x.Id == request.ModelExamId && x.IsFree, cancellationToken);
        }
        return new(hasValidSubscription);
    }
}
