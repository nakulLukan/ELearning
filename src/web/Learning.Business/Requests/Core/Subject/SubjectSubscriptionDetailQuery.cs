using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Domain.Subscription;
using Learning.Shared.Common.Enums;
using Learning.Shared.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class SubjectSubscriptionDetailQuery : IRequest<SubjectSubscriptionDetailDto>
{
    public int SubjectId { get; set; }
}

public class SubjectSubscriptionDetailQueryHandlerHandler : IRequestHandler<SubjectSubscriptionDetailQuery, SubjectSubscriptionDetailDto>
{
    readonly IAppDbContext _dbContext;

    public SubjectSubscriptionDetailQueryHandlerHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SubjectSubscriptionDetailDto> Handle(SubjectSubscriptionDetailQuery request, CancellationToken cancellationToken)
    {
        var subscriptionDetails = await _dbContext.SubjectSubscriptionDetails
            .Where(x => x.SubjectId == request.SubjectId)
            .Select(x => new SubjectSubscriptionDetail
            {
                SubjectId = request.SubjectId,
                ExpiryType = x.ExpiryType,
                NumOfDays = x.NumOfDays,
                ExpiryAbsoluteDate = x.ExpiryAbsoluteDate,
                DiscountedPrice = x.DiscountedPrice,
                OriginalPrice = x.OriginalPrice,
                ExpiryDate = x.ExpiryDate
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (subscriptionDetails == null)
        {
            return new()
            {
                DiscountedPrice = null,
                SubscriptionType = "-"
            };
        }
        return new()
        {
            SubscriptionType = GetSubscriptionTypeName(subscriptionDetails.ExpiryType, subscriptionDetails.NumOfDays ?? 0, subscriptionDetails.ExpiryAbsoluteDate ?? default, subscriptionDetails.ExpiryDate ?? default),
            DiscountedPrice = subscriptionDetails.DiscountedPrice,
            OriginalPrice = subscriptionDetails.OriginalPrice,
            Id = subscriptionDetails.Id,
            DiscountPerc = (100F - (subscriptionDetails.DiscountedPrice / subscriptionDetails.OriginalPrice) * 100)
        };
    }

    private string GetSubscriptionTypeName(SubscriptionExpiryType expiryType, int numOfDays, DateTimeOffset absoluteExpiryDate, DateOnly expiryDate)
    {
        return expiryType switch
        {
            SubscriptionExpiryType.Yearly => $"Expires on {expiryDate.ToString("dd MMM")} of every year",
            SubscriptionExpiryType.RelativeExpiry => $"Valid for {numOfDays} days",
            SubscriptionExpiryType.AbsoluteExpiry => $"Valid till {absoluteExpiryDate.ToLocalDateString()}",
            SubscriptionExpiryType.Never => "Never Expires"
        };
    }
}

