using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Business.Services.Core;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class SubjectDetailViewByIdQuery : IRequest<SubjectDetailViewDto>
{
    public int SubjectId { get; set; }
}
public class SubjectDetailViewByIdQueryHandler : IRequestHandler<SubjectDetailViewByIdQuery, SubjectDetailViewDto>
{
    readonly IAppDbContext _dbContext;
    readonly IFileStorage _fileStorage;
    readonly SubjectManager _subjectManager;

    public SubjectDetailViewByIdQueryHandler(IAppDbContextFactory dbContext, IFileStorage fileStorage, SubjectManager subjectManager)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
        _subjectManager = subjectManager;
    }

    public async Task<SubjectDetailViewDto> Handle(SubjectDetailViewByIdQuery request, CancellationToken cancellationToken)
    {
        var details = await _dbContext.Subjects.Where(x => x.Id == request.SubjectId)
            .Select(x => new
            {
                SubjectId = x.Id,
                CreatedOn = x.CreatedOn,
                x.Code,
                SubjectName = x.Name,
                ClassName = x.Class.Name,
                CourseName = x.Class.Course.Name,
                x.ThumbnailRelativePath,
                x.Description,
                SubscriptionDetail = x.SubscriptionDetail,
                x.LastUpdatedOn,
                TotalSubjectDuration = x.Chapters.SelectMany(x => x.Lessons).Sum(x => x.Video.Duration)
            })
            .FirstAsync(cancellationToken);

        var response = new SubjectDetailViewDto
        {
            CreatedOn = details.CreatedOn.Value.ToLocalDateFormatedString(),
            DiscountedPrice = details.SubscriptionDetail.DiscountedPrice,
            Price = details.SubscriptionDetail.OriginalPrice,
            SubjectDescription = details.Description,
            SubjectCode = details.Code,
            SubjectId = request.SubjectId,
            SubjectName = details.SubjectName,
            ThumbnailUrl = _fileStorage.GetPresignedUrl(details.ThumbnailRelativePath),
            HasPurchased = await _subjectManager.IsSubjectSubscribed(request.SubjectId),
            ClassName = details.ClassName,
            CourseName = details.CourseName,
            TotalDuration = details.TotalSubjectDuration.ToDurationString(),
            LastUpdatedOn = details.LastUpdatedOn.Value.ToLocalDateFormatedString(),
        };
        // If the user has subscribed then show the expiry date. Else, show validity details of the course.
        response.Validity = response.HasPurchased ?
            SubjectManager.GetSubscriptionExpiryDateAsString(details.SubscriptionDetail) :
            SubjectManager.GetPublicUserValidityMessage(details.SubscriptionDetail);

        var expiryInvalidCheckResult = SubjectManager.CheckIfSubscriptionIsInvalid(details.SubscriptionDetail);
        response.InvalidSubscriptionMessage = expiryInvalidCheckResult.InvalidMessage;
        response.IsInvalidSubscription = expiryInvalidCheckResult.IsSubscriptionInvalid;

        return response;
    }
}
