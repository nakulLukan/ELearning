using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class AdminExamNotificationByIdQuery : IRequest<AdminExamNotificationDetailResponseDto>
{
    public int ExamNotificationId { get; set; }
}

public class AdminExamNotificationByIdQueryHandler : IRequestHandler<AdminExamNotificationByIdQuery, AdminExamNotificationDetailResponseDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public AdminExamNotificationByIdQueryHandler(IAppDbContextFactory dbContext, IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<AdminExamNotificationDetailResponseDto> Handle(AdminExamNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        var examNotification = await _dbContext.ExamNotifications
            .Where(x => x.Id == request.ExamNotificationId)
             .Select(x => new AdminExamNotificationDetailResponseDto
             {
                 CreatedOn = x.CreatedOn.Value,
                 Description = x.Description,
                 DisplayInHomePage = x.DisplayInHomePage,
                 Title = x.NotificationTitle,
                 ValidTill = x.ValidTill,
                 ImageAbsPath = _fileStorage.GetObjectUrl(x.ImageRelativePath),
                 GovtLink = x.GovtLink,
                 ExamNotificationId = x.Id,
                 ImportantPoints = x.ImportantPoints,
                 LastUpdatedOn = x.LastUpdatedOn.Value,
                 PdfFile =x.PdfFile!=null ? new Shared.Common.Dto.Storage.FileDetailDto
                 {
                     FileName = x.PdfFile.FileName,
                     FileSize = x.PdfFile.Size,
                     Source = _fileStorage.GetObjectUrl(x.PdfFile.RelativePath),
                 } : null,
                 VideoFile = x.Video != null ? new Shared.Common.Dto.Storage.FileDetailDto
                 {
                     FileName = x.Video.FileName,
                     FileSize = x.Video.Size,
                     Source = _fileStorage.GetObjectUrl(x.Video.RelativePath),
                 } : null,
             })
             .SingleOrDefaultAsync(cancellationToken) ?? throw new AppException("Invalid exam notification ID.", true);
        return examNotification;
    }
}

