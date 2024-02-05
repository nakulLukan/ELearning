using Learning.Business.Contracts.HttpContext;
using Learning.Business.Impl.Data;
using Learning.Domain.Subscription;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Learning.Business.Services.Core;

public class SubjectManager
{
    readonly IAppDbContext _dbContext;
    readonly IRequestContext _requestContext;
    readonly ILogger<SubjectManager> _logger;

    public SubjectManager(IAppDbContextFactory dbContext, IRequestContext requestContext, ILogger<SubjectManager> logger)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _logger = logger;
    }

    public async Task<bool> IsSubjectSubscribed(int subjectId, CancellationToken cancellationToken = default)
    {
        if (!await _requestContext.IsAuthenticated() || await _requestContext.IsAdmin())
        {
            return false;
        }

        var userId = await _requestContext.GetUserId();
        var subscriptionDetail = await _dbContext.UserSubscriptions
            .Where(x => x.UserId == userId && x.SubjectId == subjectId)
            .Where(x => x.ExpiresOn.Date >= AppDateTime.UtcNow.Date)
            .FirstOrDefaultAsync(cancellationToken);

        var isSubscribed = subscriptionDetail != null;
        _logger.LogInformation("SM:01 User subscription status; User Id: {UserId} Status: {Status}", userId, isSubscribed);
        return isSubscribed;
    }

    public static string GetPublicUserValidityMessage(SubjectSubscriptionDetail detail)
    {
        string validity;
        switch (detail.ExpiryType)
        {
            case Shared.Common.Enums.SubscriptionExpiryType.Never:
                validity = "Lifetime";
                break;
            case Shared.Common.Enums.SubscriptionExpiryType.AbsoluteExpiry:
                validity = "Expires on " + detail.ExpiryAbsoluteDate.Value.ToLocalDateFormatedString();
                break;
            case Shared.Common.Enums.SubscriptionExpiryType.RelativeExpiry:
                validity = detail.NumOfDays + " Days";
                break;
            case Shared.Common.Enums.SubscriptionExpiryType.Yearly:

                // If expiry date and month is less than present day then add 1 more year to keep the expiry date always in future.
                var expiryDateTime = new DateTimeOffset(AppDateTime.UtcNow.Year, detail.ExpiryDate.Value.Month, detail.ExpiryDate.Value.Day, 0, 0, 0, TimeSpan.Zero);
                if (expiryDateTime <= AppDateTime.UtcNow)
                {
                    expiryDateTime = expiryDateTime.AddYears(1);
                }

                validity = "Expires on " + expiryDateTime.ToLocalDateFormatedString();
                break;
            default: throw new AppException("Uknown subscription type.");
        };

        return validity;
    }

    public static (bool IsSubscriptionInvalid, string InvalidMessage)
        CheckIfSubscriptionIsInvalid(SubjectSubscriptionDetail detail)
    {

        // If subscription expiry type is absolute and if present date as passed then mark this subject as invalid.
        // So the user does not purchase this course.
        if (detail.ExpiryType == Shared.Common.Enums.SubscriptionExpiryType.AbsoluteExpiry
            && detail.ExpiryAbsoluteDate < AppDateTime.UtcNow)
        {
            return (true, "This course is no longer valid.");
        }

        return (false, string.Empty);
    }

    public static DateTimeOffset? GetSubscriptionExpiryDate(SubjectSubscriptionDetail detail)
    {
        switch (detail.ExpiryType)
        {
            case Shared.Common.Enums.SubscriptionExpiryType.AbsoluteExpiry: return detail.ExpiryAbsoluteDate.Value;
            case Shared.Common.Enums.SubscriptionExpiryType.Yearly:

                // If expiry date and month is less than present day then add 1 more year to keep the expiry date always in future.
                var expiryDateTime = new DateTimeOffset(AppDateTime.UtcNow.Year, detail.ExpiryDate.Value.Month, detail.ExpiryDate.Value.Day, 0, 0, 0, TimeSpan.Zero);
                if (expiryDateTime <= AppDateTime.UtcNow)
                {
                    expiryDateTime = expiryDateTime.AddYears(1);
                }

                return expiryDateTime;
            case Shared.Common.Enums.SubscriptionExpiryType.RelativeExpiry:
                return AppDateTime.UtcNow.AddDays(detail.NumOfDays.Value);
            case Shared.Common.Enums.SubscriptionExpiryType.Never:
                return null;
            default: throw new AppException("Unknown subscription type");
        }
    }

    public static string GetSubscriptionExpiryDateAsString(SubjectSubscriptionDetail detail)
    {
        var expDate = GetSubscriptionExpiryDate(detail);
        if (expDate == null)
        {
            return "Lifetime Access";
        }

        return expDate.Value.ToLocalDateFormatedString();
    }
}
