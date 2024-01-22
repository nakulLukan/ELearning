using Learning.Business.Dto.Master.Lookup;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Master.Lookup;

public class AddLookupValueCommand : AddLookupValueDto, IRequest<ApiResponseDto<int>>
{
}

public class AddLookupValueCommandHandler : IRequestHandler<AddLookupValueCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public AddLookupValueCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ApiResponseDto<int>> Handle(AddLookupValueCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.LookupValues
            .AnyAsync(x => x.LookupMasterId == request.LookupMasterId
                && (request.DisplayValue == x.DisplayValue
                    || x.InternalName == request.InternalName
                        && !string.IsNullOrEmpty(x.InternalName)), cancellationToken);
        if (exists)
        {
            throw new AppException("Another item with same name or code already exists for the given category.");
        }

        var lastOrder = await _dbContext.LookupValues
            .OrderByDescending(x => x.Order)
            .Where(x => x.LookupMasterId == request.LookupMasterId)
            .Select(x => x.Order)
            .FirstOrDefaultAsync(cancellationToken);

        var currTime = AppDateTime.UtcNow;
        var lookupValue = new Domain.Master.LookupValue
        {
            InternalName = request.InternalName,
            DisplayValue = request.DisplayValue,
            LookupMasterId = request.LookupMasterId.Value,
            LastUpdatedOn = currTime,
            CreatedOn = currTime,
            IsActive = request.IsActive,
            Order = lastOrder + 1
        };

        _dbContext.LookupValues.Add(lookupValue);
        await _dbContext.SaveAsync(cancellationToken);

        return new(lookupValue.Id);
    }
}

