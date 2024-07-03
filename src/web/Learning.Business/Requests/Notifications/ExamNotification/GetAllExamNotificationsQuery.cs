using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class GetAllExamNotificationsQuery : IRequest<List<ExamNotificationsListItemDto>>
{
}

public class GetAllExamNotificationsQueryHandler : IRequestHandler<GetAllExamNotificationsQuery, List<ExamNotificationsListItemDto>>
{
    private readonly IAppCache _appCache;
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public GetAllExamNotificationsQueryHandler(IAppCache appCache, IAppDbContextFactory dbContext, IFileStorage fileStorage)
    {
        _appCache = appCache;
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<List<ExamNotificationsListItemDto>> Handle(GetAllExamNotificationsQuery request, CancellationToken cancellationToken)
    {
        var examNotifications = await _dbContext.ExamNotifications
            .OrderBy(x => x.ValidTill >= DateOnly.FromDateTime(DateTime.UtcNow))
                .ThenBy(x => x.DisplayInHomePage)
                    .ThenBy(x => x.CreatedOn)
                        .ThenBy(x => x.NotificationTitle)
             .Select(x => new ExamNotificationsListItemDto
             {
                 CreatedOn = x.CreatedOn.Value,
                 DisplayInHomePage = x.DisplayInHomePage,
                 Title = x.NotificationTitle,
                 ValidTill = x.ValidTill,
                 ImageStaticPath = x.ImageRelativePath,
                 Id = x.Id
             })
             .ToListAsync(cancellationToken);

        int index = 1;
        examNotifications.ForEach(x =>
        {
            x.Index = index++;
            x.ImageStaticPath = _fileStorage.GetObjectUrl(x.ImageStaticPath);
        });

        return examNotifications;
    }
}

