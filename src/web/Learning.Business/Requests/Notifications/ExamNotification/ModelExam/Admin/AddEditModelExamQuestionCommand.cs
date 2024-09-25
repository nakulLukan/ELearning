using Learning.Business.Dto.Notifications.ExamNotification.ModelExam.Admin;
using Learning.Business.Impl.Data;
using Learning.Domain.Notification;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using Learning.Shared.Common.Utilities;
using Learning.Shared.Contracts.HttpContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification.ModelExam.Admin;

public class AddEditModelExamQuestionCommand : IRequest<ResponseDto<int>>
{
    public required int ExamNotificationId { get; set; }
    public required int ModelExamId { get; set; }
    public required int QuestionId { get; set; }
    public required string? QuestionText { get; set; }
    public required byte[]? QuestionImage { get; set; }
    public required int Score { get; set; }
    public required int QuestionNumber { get; set; }
    public required bool IsActive { get; set; }
    public required List<AddEditModelExamOptionDto> Options { get; set; }
}
public class AddEditModelExamQuestionCommandHandler : IRequestHandler<AddEditModelExamQuestionCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IRequestContext _requestContext;
    private readonly IFileStorage _fileStorage;

    public AddEditModelExamQuestionCommandHandler(
        IAppDbContextFactory dbContext,
        IRequestContext requestContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _requestContext = requestContext;
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<int>> Handle(AddEditModelExamQuestionCommand request, CancellationToken cancellationToken)
    {
        // Get default quiz or given quiz id.
        // If quiz id is null then take default quiz
        var question = await _dbContext.ModelExamQuestionConfigurations.AsTracking()
            .Include(x=>x.QuestionImage)
            .Include(x => x.ModelExamAnswers!)
                .ThenInclude(x => x.AnswerImage)
            .FirstOrDefaultAsync(x => request.ModelExamId == x.ExamConfigId
                && request.QuestionId == x.Id, cancellationToken);
        var orderExists = await _dbContext.ModelExamQuestionConfigurations
            .AnyAsync(x => x.ExamConfigId == request.ModelExamId
            && x.Id != request.QuestionId
            && x.Order == request.QuestionNumber && x.IsActive, cancellationToken);
        if (orderExists)
        {
            throw new AppException("Question with same number already exists. Please choose a unique value.");
        }
        var questionImageInfo = await GetQuestionImagePath(request, cancellationToken);
        if (question == null)
        {
            question = await CreateNewQuestion(request, questionImageInfo, cancellationToken);
        }
        else
        {
            UpdateQuestion(request, question, questionImageInfo);
        }

        await UpdateOptions(request, question, cancellationToken);

        await _dbContext.SaveAsync(cancellationToken);
        return new(question.Id);
    }

    private async Task<AttachmentInfoDto?> GetQuestionImagePath(AddEditModelExamQuestionCommand request, CancellationToken cancellationToken)
    {
        AttachmentInfoDto? questionImageInfo = null;
        if (request.QuestionImage != null)
        {
            string fileRelativePath = StoragePathConstant.ExamNotificationModelExamBasePath(request.ExamNotificationId, request.ModelExamId);
            string fileName = $"question_{request.QuestionNumber}.jpeg";
            _ = (await _fileStorage.UploadFileToPrivate(request.QuestionImage, fileName, fileRelativePath, cancellationToken)).RelativePath;
            questionImageInfo = new(fileName, request.QuestionImage.LongLength, fileRelativePath, false);
        }

        return questionImageInfo;
    }

    private async Task UpdateOptions(
        AddEditModelExamQuestionCommand request,
        ModelExamQuestionConfiguration existingQuestion,
        CancellationToken cancellationToken)
    {
        foreach (var option in request.Options)
        {
            var order = request.Options.IndexOf(option) + 1;
            var existingAnswer = existingQuestion.ModelExamAnswers!.SingleOrDefault(x => x.Order == order);
            AttachmentInfoDto? optionImageInfo = null;
            if (option.AnswerImage != null)
            {
                string filePath = StoragePathConstant.ExamNotificationModelExamBasePath(request.ExamNotificationId, request.ModelExamId);
                string fileName = $"question_{existingQuestion.Order}_{option.OptionOrder}.jpeg";
                string optionImagePath = (await _fileStorage.UploadFileToPrivate(option.AnswerImage, fileName, filePath, cancellationToken)).RelativePath;
                optionImageInfo = new(fileName, option.AnswerImage.LongLength, filePath, false);
            }
            if (existingAnswer == null)
            {
                existingAnswer = CreateOption(request, existingQuestion, option, order, optionImageInfo);
            }
            else
            {
                UpdateOption(request, option, existingAnswer, order, optionImageInfo);
            }
        }
    }

    private static void UpdateOption(AddEditModelExamQuestionCommand request,
                                     AddEditModelExamOptionDto option,
                                     ModelExamAnswerConfiguration existingAnswer,
                                     int order,
                                     AttachmentInfoDto? optionImageInfo)
    {
        if (existingAnswer.AnswerText != option.AnswerText)
        {
            existingAnswer.AnswerText = option.AnswerText;
            existingAnswer.AnswerImageId = null;
        }

        if (option.AnswerImage != null)
        {
            existingAnswer.AnswerImage = existingAnswer.AnswerImage ?? new Domain.Master.Attachment
            {
                FileName = string.Empty,
                RelativePath = string.Empty,
                Size = 0
            };
            existingAnswer.AnswerImage.RelativePath = optionImageInfo!.RelativePath;
            existingAnswer.AnswerImage.FileName = optionImageInfo.FileName;
            existingAnswer.AnswerImage.Size = optionImageInfo.FileSize;
            existingAnswer.AnswerText = null;
        }
        existingAnswer.IsCorrectAnswer = option.IsCorrectOption;
    }

    private static ModelExamAnswerConfiguration CreateOption(
        AddEditModelExamQuestionCommand request,
        ModelExamQuestionConfiguration existingQuestion,
        AddEditModelExamOptionDto option,
        int order,
        AttachmentInfoDto? optionImageInfo)
    {
        ModelExamAnswerConfiguration existingAnswer = new()
        {
            Id = 0,
            AnswerText = option.AnswerText,
            AnswerType = Shared.Common.Enums.AnswerType.Mcq,
            IsCorrectAnswer = option.IsCorrectOption,
            AnswerImageId = null,
            Order = option.OptionOrder,
            QuestionId = existingQuestion.Id,
            AnswerImage = optionImageInfo != null ?
                            new Domain.Master.Attachment()
                            {
                                FileName = Path.GetFileName(optionImageInfo.FileName),
                                RelativePath = optionImageInfo.RelativePath,
                                Size = optionImageInfo.FileSize,
                            } : null,
        };
        existingQuestion.ModelExamAnswers!.Add(existingAnswer);
        return existingAnswer;
    }

    private static void UpdateQuestion(
        AddEditModelExamQuestionCommand request,
        ModelExamQuestionConfiguration existingQuestion,
        AttachmentInfoDto? questionImageInfo)
    {
        if (existingQuestion.Score != request.Score)
        {
            existingQuestion.Score = request.Score;
        }
        if (existingQuestion.IsActive != request.IsActive)
        {
            existingQuestion.IsActive = request.IsActive;
        }
        if (existingQuestion.Order != request.QuestionNumber)
        {
            existingQuestion.Order = request.QuestionNumber;
        }
        if (existingQuestion.QuestionText != request.QuestionText)
        {
            existingQuestion.QuestionText = request.QuestionText;
        }
        if (questionImageInfo != null)
        {
            if (existingQuestion.QuestionImage == null)
            {
                existingQuestion.QuestionImage = new()
                {
                    FileName = questionImageInfo.FileName,
                    RelativePath = questionImageInfo.RelativePath,
                    CreatedOn = AppDateTime.UtcNow,
                    Size = questionImageInfo.FileSize,
                };
            }
            else if (existingQuestion.QuestionImage.Size != questionImageInfo.FileSize
                    || existingQuestion.QuestionImage.RelativePath != questionImageInfo.RelativePath)
            {

                existingQuestion.QuestionImage.FileName = questionImageInfo.FileName;
                existingQuestion.QuestionImage.RelativePath = questionImageInfo.RelativePath;
                existingQuestion.QuestionImage.Size = questionImageInfo.FileSize;
            }
        }
    }

    private async Task<ModelExamQuestionConfiguration> CreateNewQuestion(AddEditModelExamQuestionCommand request,
                                                       AttachmentInfoDto? questionImageRelativePath,
                                                       CancellationToken cancellationToken)
    {
        ModelExamQuestionConfiguration existingQuestion = new()
        {
            Id = request.QuestionId,
            Score = request.Score,
            Order = request.QuestionNumber,
            QuestionText = request.QuestionText,
            ExamConfigId = request.ModelExamId,
            QuestionImage = questionImageRelativePath != null ? new Domain.Master.Attachment()
            {
                FileName = questionImageRelativePath.FileName,
                RelativePath = questionImageRelativePath.RelativePath,
                Size = questionImageRelativePath.FileSize,
                CreatedOn = AppDateTime.UtcNow,
            } : null,
            CreatedOn = AppDateTime.UtcNow,
            IsActive = request.IsActive,
            LastUpdatedOn = AppDateTime.UtcNow,
            ModelExamAnswers = new()
        };
        _dbContext.ModelExamQuestionConfigurations.Add(existingQuestion);
        await _dbContext.SaveAsync(cancellationToken);
        return existingQuestion;
    }
}

public class AttachmentInfoDto
{
    public string RelativePath { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public AttachmentInfoDto(string fileName, long fileSize, string relativePath, bool isFullPath = true)
    {
        FileName = fileName;
        FileSize = fileSize;
        RelativePath = isFullPath ? relativePath : relativePath + "/" + fileName;
    }
}
