using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Business.Services.Core;
using Learning.Domain.Subscription;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject.Purchase;

public class SubjectBuyCommand : IRequest<ApiResponseDto<long>>
{
    public int SubjectId { get; set; }
}

public class SubjectBuyCommandHandler : IRequestHandler<SubjectBuyCommand, ApiResponseDto<long>>
{
    readonly IAppDbContext _dbContext;
    readonly IRequestContext _requestContext;

    public SubjectBuyCommandHandler(IAppDbContextFactory dbContext, IRequestContext requestContext)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<ApiResponseDto<long>> Handle(SubjectBuyCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        await ThrowIfAlreadyPurchased(request, userId, cancellationToken);
        UserSubscription newSubscription = await AddSubscription(request, userId, cancellationToken);

        return new(newSubscription.Id);
    }

    private async Task<UserSubscription> AddSubscription(SubjectBuyCommand request, string userId, CancellationToken cancellationToken)
    {
        var subjectSubscriptionDetails = await _dbContext.Subjects
                    .Where(x => x.Id == request.SubjectId)
                    .Select(x => x.SubscriptionDetail)
                    .FirstAsync(cancellationToken);

        var subscriptionExpiry = SubjectManager.GetSubscriptionExpiryDate(subjectSubscriptionDetails);
        UserSubscription newSubscription = new()
        {
            CreatedOn = AppDateTime.UtcNow,
            ExpiresOn = subscriptionExpiry ?? DateTimeOffset.MaxValue,
            Price = subjectSubscriptionDetails.DiscountedPrice,
            SubjectId = request.SubjectId,
            UserId = userId
        };

        _dbContext.UserSubscriptions.Add(newSubscription);
        await _dbContext.SaveAsync(cancellationToken);
        return newSubscription;
    }

    private async Task ThrowIfAlreadyPurchased(SubjectBuyCommand request, string userId, CancellationToken cancellationToken)
    {
        var subscriptionDetail = await _dbContext.UserSubscriptions
                    .Where(x => x.UserId == userId
                        && x.SubjectId == request.SubjectId
                        && x.ExpiresOn >= AppDateTime.UtcNow)
                    .SingleOrDefaultAsync(cancellationToken);

        if (subscriptionDetail != null)
        {
            throw new AppException("This course is already purchased.");
        }
    }
}

