using FluentValidation;
using Learning.Business.Dto.Core.Subject;
using Learning.Business.Impl.Data;
using Learning.Business.Impl.Validator;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Subject;

public class UpdateSubjectSubscriptionDetailCommand : UpdateSubjectSubscriptionDto, IRequest<ApiResponseDto<int>>
{
}

public class UpdateSubjectSubscriptionDetailCommandHandler : IRequestHandler<UpdateSubjectSubscriptionDetailCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public UpdateSubjectSubscriptionDetailCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponseDto<int>> Handle(UpdateSubjectSubscriptionDetailCommand request, CancellationToken cancellationToken)
    {
        var subscriptionDetail = await _dbContext.SubjectSubscriptionDetails.AsTracking()
            .Where(x => x.SubjectId == request.SubjectId)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscriptionDetail == null)
        {
            subscriptionDetail = new() { SubjectId = request.SubjectId };
            _dbContext.SubjectSubscriptionDetails.Add(subscriptionDetail);
            await _dbContext.SaveAsync(cancellationToken);
        }

        subscriptionDetail.OriginalPrice = request.OriginalPrice.Value;
        subscriptionDetail.DiscountedPrice = request.DiscountedPrice.Value;
        subscriptionDetail.ExpiryType = request.SubscriptionType.Value;
        subscriptionDetail.NumOfDays = request.NumOfDays;
        subscriptionDetail.ExpiryAbsoluteDate = request.ExpiryAbsoluteDate;
        subscriptionDetail.ExpiryDate = request.ExpiryDate.HasValue ? DateOnly.FromDateTime(request.ExpiryDate.Value) : null;

        await _dbContext.SaveAsync(cancellationToken);
        return new(subscriptionDetail.Id);
    }
}

public class UpdateSubjectSubscriptionDetailCommandValidator : AppAbstractValidator<UpdateSubjectSubscriptionDto>
{
    public UpdateSubjectSubscriptionDetailCommandValidator()
    {
        RuleFor(x => x.OriginalPrice)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DiscountedPrice)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DiscountedPrice)
            .LessThanOrEqualTo(x => x.OriginalPrice)
            .WithMessage("'Discounted Price' cannot be greater than 'Price'")
            .When(x => x.OriginalPrice.HasValue);

        RuleFor(x => x.SubscriptionType)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.NumOfDays)
            .NotNull()
            .When(x => x.SubscriptionType == Shared.Common.Enums.SubscriptionExpiryType.RelativeExpiry);

        RuleFor(x => x.NumOfDays)
            .Must(x => x.HasValue && x.Value > 0)
            .When(x => x.SubscriptionType == Shared.Common.Enums.SubscriptionExpiryType.RelativeExpiry);

        RuleFor(x => x.ExpiryDate)
            .Must(x => x.HasValue)
            .When(x => x.SubscriptionType == Shared.Common.Enums.SubscriptionExpiryType.Yearly);

        RuleFor(x => x.ExpiryAbsoluteDate)
            .Must(x => x.HasValue)
            .When(x => x.SubscriptionType == Shared.Common.Enums.SubscriptionExpiryType.AbsoluteExpiry);

    }
}
