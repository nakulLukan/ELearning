using Learning.Business.Contracts.HttpContext;
using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Business.Services.Core;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject.Purchase;

public class SubjectPurchaseDetailsQuery : IRequest<SubjectPurchaseDetailDto>
{
    /// <summary>
    /// Subject short code
    /// </summary>
    public string SubjectCode { get; set; }
}

public class SubjectPurchaseDetailsQueryHandler : IRequestHandler<SubjectPurchaseDetailsQuery, SubjectPurchaseDetailDto>
{
    readonly IAppDbContext _dbContext;
    readonly IRequestContext _requestContext;

    public SubjectPurchaseDetailsQueryHandler(IAppDbContextFactory dbContext, IRequestContext requestContext)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
    }

    public async Task<SubjectPurchaseDetailDto> Handle(SubjectPurchaseDetailsQuery request, CancellationToken cancellationToken)
    {
        var subjectDetails = await _dbContext.Subjects.Where(x => x.Code == request.SubjectCode)
            .Select(x => new
            {
                x.Id,
                SubjectName = x.Name,
                ClassName = x.Class.Name,
                CourseName = x.Class.Course.Name,
                x.SubscriptionDetail,
            })
            .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Unknown Course.", true);
        SubjectPurchaseDetailDto response = new()
        {
            BuyPrice = subjectDetails.SubscriptionDetail.DiscountedPrice,
            ClassName = subjectDetails.ClassName,
            CourseName = subjectDetails.CourseName,
            SubjectId = subjectDetails.Id,
            SubjectName = subjectDetails.SubjectName,
        };

        // Set course expiry date
        var endsOnDate = SubjectManager.GetSubscriptionExpiryDate(subjectDetails.SubscriptionDetail);
        if (!endsOnDate.HasValue)
        {
            response.SubscriptionEndDate = "Lifetime Access";
        }
        else
        {
            response.SubscriptionEndDate = endsOnDate.Value.ToLocalDateFormatedString();
        }

        // Check if the user has already purchased.
        var userId = await _requestContext.GetUserId();
        var subscriptionDetail = await _dbContext.UserSubscriptions
            .Where(x => x.UserId == userId
                && x.SubjectId == subjectDetails.Id
                && x.ExpiresOn >= AppDateTime.UtcNow)
            .SingleOrDefaultAsync(cancellationToken);

        if (subscriptionDetail != null)
        {
            response.Result = Shared.Common.Enums.SubjectPurchaseValidationEnum.AlreadyPurchased;
        }
        else
        {
            response.Result = Shared.Common.Enums.SubjectPurchaseValidationEnum.Success;
        }

        return response;
    }
}

