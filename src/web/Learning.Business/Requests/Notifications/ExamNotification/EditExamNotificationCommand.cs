using Learning.Business.Constants.Notifications;
using Learning.Business.Contracts.HttpContext;
using Learning.Business.Contracts.Persistence;
using Learning.Business.Dto.Notifications.ExamNotification;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Dto.File;
using Learning.Shared.Common.Extensions;
using Learning.Shared.Common.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class EditExamNotificationCommand : AddEditExamNotificationRequestDto, IRequest<ResponseDto<int>>
{
    public required int ExamNotificationId { get; set; }
    public FileStreamData ImageFile { get; set; }
}

public class EditExamNotificationCommandHandler : IRequestHandler<EditExamNotificationCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IAppCache _appCache;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<EditExamNotificationCommandHandler> _logger;

    public EditExamNotificationCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IAppCache appCache,
        IFileStorage fileStorage,
        ILogger<EditExamNotificationCommandHandler> logger)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _appCache = appCache;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<ResponseDto<int>> Handle(EditExamNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();
        Domain.Notification.ExamNotification existingNotification =
            await GetExistingExamNotification(request, cancellationToken);

        // Update only changed fields
        await UpdateFields(request, userId, existingNotification, cancellationToken);
        await _dbContext.SaveAsync(cancellationToken);

        // Reset cache
        _appCache.DeleteKey(ExamNotificationCacheKey.ActiveNotificationsKey);
        return new(request.ExamNotificationId);
    }

    private async Task UpdateFields(
        EditExamNotificationCommand request,
        string userId,
        Domain.Notification.ExamNotification existingNotification,
        CancellationToken cancellationToken)
    {
        if (existingNotification.NotificationTitle != request.Title)
        {
            existingNotification.NotificationTitle = request.Title;
        }
        if (existingNotification.Description != request.Description)
        {
            existingNotification.Description = request.Description;
        }

        if (existingNotification.ImportantPoints != request.ImportantPoints)
        {
            existingNotification.ImportantPoints = request.ImportantPoints;
        }

        if (existingNotification.DisplayInHomePage != request.DisplayInHomePage)
        {
            existingNotification.DisplayInHomePage = request.DisplayInHomePage;
        }

        if (existingNotification.GovtLink != request.GovtLink)
        {
            existingNotification.GovtLink = request.GovtLink;
        }

        if (existingNotification.ValidTill != request.ValidTill.ToDateOnly())
        {
            existingNotification.ValidTill = request.ValidTill.ToDateOnly();
        }
        if (request.ImageFile != null)
        {
            var relativePath = await UploadImageToStorage(existingNotification, request, cancellationToken);
            existingNotification.ImageRelativePath = relativePath;
        }

        existingNotification.LastUpdatedOn = AppDateTime.UtcNow;
        existingNotification.LastUpdatedBy = userId;
    }

    private async Task<Domain.Notification.ExamNotification> GetExistingExamNotification(EditExamNotificationCommand request, CancellationToken cancellationToken)
    {
        return await _dbContext.ExamNotifications.AsTracking()
                    .Where(x => x.Id == request.ExamNotificationId)
                    .Include(x => x.Video)
                    .Include(x => x.PdfFile)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new AppException("Exam notification not found");
    }

    private async Task<string> UploadImageToStorage(Domain.Notification.ExamNotification existingNotification, EditExamNotificationCommand request, CancellationToken cancellationToken)
    {
        // Delete existing
        await _fileStorage.DeleteFileAsync(_fileStorage.GetObjectUrl(existingNotification.ImageRelativePath));
        _logger.LogInformation("Lid:EN001 : image {imagePath} deleted; exam notification id: {examNotification}", existingNotification.ImageRelativePath, existingNotification.Id);

        using var ms = new MemoryStream();
        await request.ImageFile.Stream.CopyToAsync(ms);
        var data = await _fileStorage.UploadFileToPublic(ms.ToArray(), request.ImageFile.FileName, StoragePathConstant.PublicExamNotificationBasePath, cancellationToken);
        return data.RelativePath;
    }
}

