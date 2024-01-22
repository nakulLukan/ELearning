using Learning.Business.Dto.Master.Lookup;
using Learning.Shared.Common.Enums;
using MediatR;

namespace Learning.Business.Requests.Master.Subscription;

public class SubscriptionExpirySelectQuery : IRequest<List<LookupSelectDto>>
{
}

public class SubscriptionExpirySelectQueryHandler : IRequestHandler<SubscriptionExpirySelectQuery, List<LookupSelectDto>>
{
    public async Task<List<LookupSelectDto>> Handle(SubscriptionExpirySelectQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new List<LookupSelectDto>()
        {
            new()
            {
                Id = (int)SubscriptionExpiryType.Yearly,
                Value = "Annual Expiry"
            },
            new()
            {
                Id = (int)SubscriptionExpiryType.RelativeExpiry,
                Value = "Expires After 'n' Days"
            },
            new()
            {
                Id = (int)SubscriptionExpiryType.AbsoluteExpiry,
                Value = "Expires Exactly On"
            },
            new()
            {
                Id = (int)SubscriptionExpiryType.Never,
                Value = "Never"
            },
        });
    }
}
