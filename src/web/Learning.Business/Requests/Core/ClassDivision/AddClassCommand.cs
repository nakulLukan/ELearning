using Learning.Business.Dto.Core.ClassDivision;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class AddClassCommand : AddClassDto, IRequest<ApiResponseDto<int>>
{
}
public class AddClassCommandHandler : IRequestHandler<AddClassCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public AddClassCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ApiResponseDto<int>> Handle(AddClassCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Classes.AnyAsync(x => request.ShortCode == x.Code || (x.CourseId == request.CourseId && x.Name == request.ClassName), cancellationToken);
        if (exists)
        {
            throw new AppException("Another course with same short code already exists. Please give a unique value.");
        }

        var currTime = AppDateTime.UtcNow;
        var classDivision = new Domain.Core.ClassDivision
        {
            Name = request.ClassName,
            Code = request.ShortCode,
            Description = request.Description,
            IsActive = request.IsActive,
            CreatedOn = currTime,
            LastUpdatedOn = currTime,
            CourseId = request.CourseId,
        };

        _dbContext.Classes.Add(classDivision);
        await _dbContext.SaveAsync(cancellationToken);

        return new(classDivision.Id);
    }
}

