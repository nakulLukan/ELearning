using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Dto.File;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Contracts.HttpContext;
using MediatR;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class AddExamNotificationCommand : AddEditExamNotificationRequestDto, IRequest<ResponseDto<int>>
{
    public FileStreamData ImageFile { get; set; }
}

public class AddExamNotificationCommandHandler : IRequestHandler<AddExamNotificationCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IAppCache _appCache;
    private readonly IFileStorage _fileStorage;

    public AddExamNotificationCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IAppCache appCache,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _appCache = appCache;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<int>> Handle(AddExamNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        var newExamNotification = new Domain.Notification.ExamNotification()
        {
            Description = request.Description.Trim(),
            NotificationTitle = request.Title.Trim(),
            ImageRelativePath = string.Empty,
            DisplayInHomePage = request.DisplayInHomePage,
            ValidTill = request.ValidTill.HasValue ? DateOnly.FromDateTime(request.ValidTill.Value) : null,
            CreatedBy = userId,
            LastUpdatedBy = userId,
            CreatedOn = AppDateTime.UtcNow,
            LastUpdatedOn = AppDateTime.UtcNow,
            GovtLink = request.GovtLink,
            ImportantPoints = request.ImportantPoints?.RemoveEmptyLines(),
        };

        _dbContext.ExamNotifications.Add(newExamNotification);
        await _dbContext.SaveAsync(cancellationToken);
        var relativePath = await UploadImageToStorage(request, newExamNotification.Id, cancellationToken);
        newExamNotification.ImageRelativePath = relativePath;
        await _dbContext.SaveAsync(cancellationToken);

        _appCache.DeleteKey(ExamNotificationCacheKey.ActiveNotificationsKey);
        _appCache.DeleteKey(ExamNotificationCacheKey.ActiveNotificationsDetailKey);
        return new(newExamNotification.Id);
    }

    private async Task<string> UploadImageToStorage(AddExamNotificationCommand request, int id, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await request.ImageFile.Stream.CopyToAsync(ms);
        var data = await _fileStorage.UploadFileToPublic(ms.ToArray(), request.ImageFile.FileName, StoragePathConstant.PublicExamNotificationBasePath(id), cancellationToken);
        return data.RelativePath;
    }
}

