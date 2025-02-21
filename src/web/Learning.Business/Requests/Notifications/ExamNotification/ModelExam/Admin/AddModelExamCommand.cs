using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Contracts.HttpContext;
using Learning.Shared.Dto.Notifications.ExamNotification.ModelExam.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class AddModelExamCommand : AddModelExamRequestDto, IRequest<ResponseDto<int>>
{
}
public class AddModelExamCommandHandler : IRequestHandler<AddModelExamCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IFileStorage _fileStorage;

    public AddModelExamCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<int>> Handle(AddModelExamCommand request, CancellationToken cancellationToken)
    {
        var userId = await _requestContext.GetUserId();

        var isExisting = await _dbContext.ModelExamConfigurations.AnyAsync(x =>
            x.ExamNotificationId == request.ExamNotificationId
            && x.Id != request.Id
            && x.ExamName.ToLower() == request.ExamName.ToLower(), cancellationToken);
        if (isExisting)
        {
            throw new AppException("Exam notification with same name already exists");
        }

        ModelExamConfiguration? modelExam = await _dbContext.ModelExamConfigurations.AsTracking()
            .Include(x => x.ModelExamPackage)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (modelExam == null)
        {
            modelExam = await AddModelExam(request, userId, cancellationToken);
        }
        else
        {
            EditModelExam(request, userId, modelExam!);
        }

        await _dbContext.SaveAsync(cancellationToken);
        await ValidateAndSetSolutionFile(request, modelExam, cancellationToken);
        return new(modelExam.Id);
    }

    private async Task ValidateAndSetSolutionFile(AddModelExamCommand request, ModelExamConfiguration? modelExam, CancellationToken cancellationToken)
    {
        var fileBasePath = StoragePathConstant.ModelExamSolutionVideoBasPath(request.ExamNotificationId, modelExam!.Id) + "/";
        await _fileStorage.CreateDirectory(fileBasePath);
        if (request.SolutionFileName != null)
        {
            var files = await _fileStorage.GetFileNames(fileBasePath);
            var solutionFile = files.FirstOrDefault(x => x.ToLowerInvariant() == request.SolutionFileName.Trim().ToLowerInvariant());
            if (solutionFile != null)
            {
                if (modelExam.ExamSolutionVideo == null)
                {
                    modelExam.ExamSolutionVideo = new Domain.Master.Attachment
                    {
                        FileName = solutionFile,
                        RelativePath = fileBasePath + solutionFile
                    };
                }
                else
                {
                    modelExam.ExamSolutionVideo.FileName = solutionFile;
                    modelExam.ExamSolutionVideo.RelativePath = fileBasePath + solutionFile;
                }
                await _dbContext.SaveAsync(cancellationToken);
            }
            else
            {
                throw new AppException("Failed to map solution video.");
            }
        }
    }

    private static void EditModelExam(AddModelExamCommand request, string userId, ModelExamConfiguration modelExam)
    {
        if (modelExam.IsActive != request.IsActive)
        {
            modelExam.IsActive = request.IsActive;
        }

        if (modelExam.Description != request.ExamDescription)
        {
            modelExam.Description = request.ExamDescription.Trim();
        }
        if (modelExam.ModelExamPackage!.Price != request.Price)
        {
            modelExam.ModelExamPackage.Price = request.Price;
        }
        if (modelExam.ModelExamPackage.DiscountedPrice != request.DiscountedPrice)
        {
            modelExam.ModelExamPackage.DiscountedPrice = request.DiscountedPrice;
        }
        if (modelExam.ExamName != request.ExamName)
        {
            modelExam.ExamName = request.ExamName.Trim();
        }
        if (modelExam.IsFree != request.IsFree)
        {
            modelExam.IsFree = request.IsFree;
        }
        if (modelExam.TotalTimeLimit != request.TotalTimeLimit)
        {
            modelExam.TotalTimeLimit = request.TotalTimeLimit;
        }
        if (modelExam.Score != request.Score)
        {
            modelExam.Score = request.Score;
        }
        if (modelExam.NegativeScore != request.NegativeScore)
        {
            modelExam.NegativeScore = request.NegativeScore;
        }
        modelExam.LastUpdatedBy = userId;
        modelExam.LastUpdatedOn = AppDateTime.UtcNow;
    }

    private async Task<ModelExamConfiguration> AddModelExam(AddModelExamCommand request, string userId, CancellationToken cancellationToken)
    {
        var modelExamPackage = await _dbContext.ModelExamPackages.AsTracking()
            .FirstOrDefaultAsync(x => x.ExamNotificationId == request.ExamNotificationId);
        if (modelExamPackage == null)
        {
            modelExamPackage = new ModelExamPackage()
            {
                ExamNotificationId = request.ExamNotificationId,
                DiscountedPrice = request.DiscountedPrice,
                Price = request.Price,
            };

            _dbContext.ModelExamPackages.Add(modelExamPackage);
            await _dbContext.SaveAsync(cancellationToken);
        }
        else
        {
            modelExamPackage.Price = request.Price;
            modelExamPackage.DiscountedPrice = request.DiscountedPrice;
        }

        ModelExamConfiguration? modelExam = new ModelExamConfiguration()
        {
            Id = 0,
            IsActive = request.IsActive,
            CreatedBy = userId,
            CreatedOn = AppDateTime.UtcNow,
            Description = request.ExamDescription.Trim(),
            ExamName = request.ExamName.Trim(),
            ExamNotificationId = request.ExamNotificationId,
            IsFree = request.IsFree,
            LastUpdatedBy = userId,
            LastUpdatedOn = AppDateTime.UtcNow,
            TotalTimeLimit = request.TotalTimeLimit,
            ModelExamPackageId = modelExamPackage.Id,
            Score = request.Score,
            NegativeScore = request.NegativeScore,
        };
        _dbContext.ModelExamConfigurations.Add(modelExam);
        return modelExam;
    }
}

