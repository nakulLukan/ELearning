using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Subscription.Offer;

public class SetCouponCodeAsUsedCommand : IRequest<ResponseDto<bool>>
{
    public required long CouponCodeId { get; set; }
}

public class SetCouponCodeAsUsedCommandHandler : IRequestHandler<SetCouponCodeAsUsedCommand, ResponseDto<bool>>
{
    private readonly IAppDbContext _dbContext;

    public SetCouponCodeAsUsedCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ResponseDto<bool>> Handle(SetCouponCodeAsUsedCommand request, CancellationToken cancellationToken)
    {
        var updated = await _dbContext.CouponCodes
            .Where(x => x.Id == request.CouponCodeId)
            .ExecuteUpdateAsync(x =>
                x.SetProperty(prop => prop.IsUsed, true)
                .SetProperty(prop => prop.CouponUsedOn, AppDateTime.UtcNow),
                cancellationToken);
        return new(updated > 0);
    }
}
