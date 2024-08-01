using Learning.Business.Dto.Quiz.QuickTest;
using Learning.Business.Impl.Data;
using Learning.Domain.Quiz;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Common.Constants;
using Learning.Shared.Common.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class AddEditQuizQuestionCommand : IRequest<ResponseDto<int>>
{
    /// <summary>
    /// If null then use default
    /// </summary>
    public required int? QuizConfigurationId { get; set; }
    public required int QuestionId { get; set; }
    public required string Question { get; set; }
    public required byte[]? QuestionImage { get; set; }
    public required int Mark { get; set; }
    public required int TimeLimitInSeconds { get; set; }
    public required int QuestionNumber { get; set; }
    public required int CorrectOptionIndex { get; set; }
    public required List<AddEditQuizOptionDto> Options { get; set; }
}

public class AddEditQuizQuestionCommandHandler : IRequestHandler<AddEditQuizQuestionCommand, ResponseDto<int>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public AddEditQuizQuestionCommandHandler(
        IAppDbContextFactory dbContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<ResponseDto<int>> Handle(AddEditQuizQuestionCommand request, CancellationToken cancellationToken)
    {
        // Get default quiz or given quiz id.
        // If quiz id is null then take default quiz
        var quizConfig = await _dbContext.QuizConfigurations.AsTracking()
            .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.AnswerImage)
            .FirstAsync(x => 
                (request.QuizConfigurationId.HasValue && x.Id == request.QuizConfigurationId) 
                    || (!request.QuizConfigurationId.HasValue && x.IsDefault), cancellationToken);

        quizConfig.Questions = quizConfig.Questions ?? new();
        var existingQuestion = quizConfig.Questions.FirstOrDefault(x => x.Id == request.QuestionId);
        string? questionImageRelativePath = await GetQuestionImagePath(request, quizConfig, cancellationToken);
        if (existingQuestion == null)
        {
            existingQuestion = await CreateNewQuestion(request, quizConfig, questionImageRelativePath, cancellationToken);
        }
        else
        {
            UpdateQuestion(request, existingQuestion, questionImageRelativePath);
        }

        await UpdateOptions(request, existingQuestion, cancellationToken);

        await _dbContext.SaveAsync(cancellationToken);
        return new(existingQuestion.Id);
    }

    private async Task<string?> GetQuestionImagePath(AddEditQuizQuestionCommand request, QuizConfiguration quizConfig, CancellationToken cancellationToken)
    {
        string? questionImageRelativePath = null;
        if (request.QuestionImage != null)
        {
            string fileRelativePath = StoragePathConstant.QuizOptionImageBasePath(quizConfig.Id);
            string fileName = $"question_{request.QuestionNumber}.png";
            questionImageRelativePath = (await _fileStorage.UploadFileToPublic(request.QuestionImage, fileName, fileRelativePath, cancellationToken)).RelativePath;
        }

        return questionImageRelativePath;
    }

    private async Task UpdateOptions(AddEditQuizQuestionCommand request, QuizQuestion existingQuestion, CancellationToken cancellationToken)
    {
        foreach (var option in request.Options)
        {
            var order = request.Options.IndexOf(option) + 1;
            var existingAnswer = existingQuestion.Answers.SingleOrDefault(x => x.Order == order);
            string optionImagePath = string.Empty;
            if (option.AnswerImage != null)
            {
                string fileName = $"question_{existingQuestion.Id}_option_{order}.jpg";
                string filePath = StoragePathConstant.QuizOptionImageBasePath(existingQuestion.QuizConfigurationId);
                optionImagePath = (await _fileStorage.UploadFileToPublic(option.AnswerImage, fileName, filePath, cancellationToken)).RelativePath;
            }
            if (existingAnswer == null)
            {
                existingAnswer = CreateOption(request, existingQuestion, option, order, optionImagePath);
            }
            else
            {
                UpdateOption(request, option, existingAnswer, order, optionImagePath);
            }
        }
    }

    private static void UpdateOption(AddEditQuizQuestionCommand request,
                                     AddEditQuizOptionDto option,
                                     QuizQuestionAnswer existingAnswer,
                                     int order,
                                     string optionImagePath)
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
            existingAnswer.AnswerImage.RelativePath = optionImagePath;
            existingAnswer.AnswerImage.FileName = Path.GetFileName(optionImagePath);
            existingAnswer.AnswerImage.Size = option.AnswerImage.Length;
            existingAnswer.AnswerText = null;
        }
        existingAnswer.IsCorrectAnswer = order == request.CorrectOptionIndex;
    }

    private static QuizQuestionAnswer CreateOption(
        AddEditQuizQuestionCommand request,
        QuizQuestion existingQuestion,
        AddEditQuizOptionDto option,
        int order,
        string optionImagePath)
    {
        QuizQuestionAnswer existingAnswer = new QuizQuestionAnswer
        {
            Id = 0,
            AnswerText = option.AnswerText,
            AnswerType = Shared.Common.Enums.QuizAnswerType.Mcq,
            IsCorrectAnswer = order == request.CorrectOptionIndex,
            AnswerImageId = null,
            Order = order,
            QuizQuestionId = existingQuestion.Id,
            AnswerImage = !string.IsNullOrEmpty(optionImagePath) ?
                            new Domain.Master.Attachment()
                            {
                                FileName = Path.GetFileName(optionImagePath),
                                RelativePath = optionImagePath,
                                Size = optionImagePath.Length,
                            } : null,
        };
        existingQuestion.Answers.Add(existingAnswer);
        return existingAnswer;
    }

    private static void UpdateQuestion(
        AddEditQuizQuestionCommand request,
        QuizQuestion existingQuestion,
        string? questionImageRelativePath)
    {
        if (existingQuestion.Mark != request.Mark)
        {
            existingQuestion.Mark = request.Mark;
        }

        if (existingQuestion.TimeLimitInSeconds != request.TimeLimitInSeconds)
        {
            existingQuestion.TimeLimitInSeconds = request.TimeLimitInSeconds;
        }
        if (existingQuestion.Order != request.QuestionNumber)
        {
            existingQuestion.Order = request.QuestionNumber;
        }
        if (existingQuestion.Question != request.Question)
        {
            existingQuestion.Question = request.Question;
        }
        if (!string.IsNullOrEmpty(questionImageRelativePath))
        {
            existingQuestion.QuestionImageRelativePath = questionImageRelativePath;
        }
    }

    private async Task<QuizQuestion> CreateNewQuestion(AddEditQuizQuestionCommand request,
                                                       QuizConfiguration quizConfig,
                                                       string? questionImageRelativePath,
                                                       CancellationToken cancellationToken)
    {
        QuizQuestion existingQuestion = new QuizQuestion
        {
            Id = request.QuestionId,
            Mark = request.Mark,
            Order = request.QuestionNumber,
            Question = request.Question,
            TimeLimitInSeconds = request.TimeLimitInSeconds,
            QuizConfigurationId = quizConfig.Id,
            QuestionImageRelativePath = questionImageRelativePath,
            Answers = new(),
        };
        _dbContext.QuizQuestions.Add(existingQuestion);
        await _dbContext.SaveAsync(cancellationToken);
        return existingQuestion;
    }
}

