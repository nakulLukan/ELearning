using Learning.Business.Dto.Core.Course;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Core.Course;

public class AddCourseCommand : AddCourseDto, IRequest<ApiResponseDto<int>>
{
}
public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, ApiResponseDto<int>>
{
    readonly IAppDbContext _dbContext;

    public AddCourseCommandHandler(IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<ApiResponseDto<int>> Handle(AddCourseCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Courses.AnyAsync(x => request.ShortCode == x.Code, cancellationToken);
        if (exists)
        {
            throw new AppException("Another course with same short code already exists. Please give a unique value.");
        }

        var currTime = AppDateTime.UtcNow;
        var course = new Domain.Core.Course
        {
            Name = request.CourseName,
            Code = request.ShortCode,
            Description = request.Description,
            IsActive = request.IsActive,
            CreatedOn = currTime,
            LastUpdatedOn = currTime,
        };


        _dbContext.Courses.Add(course);
        await _dbContext.SaveAsync(cancellationToken);

        return new(course.Id);
    }
}

