using Learning.Business.Dto.Quiz.QuickTest;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Notifications.ExamNotification;

public class GetDefaultQuizConfigQuery : IRequest<DefaultQuizConfigResponseDto>
{
}

public class GetDefaultQuizConfigQueryHandler : IRequestHandler<GetDefaultQuizConfigQuery, DefaultQuizConfigResponseDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public GetDefaultQuizConfigQueryHandler(
        IAppDbContextFactory dbContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<DefaultQuizConfigResponseDto> Handle(GetDefaultQuizConfigQuery request, CancellationToken cancellationToken)
    {
        var defaultQuiz = await _dbContext.QuizConfigurations
            .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                    .ThenInclude(x => x.AnswerImage)
            .FirstOrDefaultAsync(x => x.IsDefault);
        if (defaultQuiz == null)
        {
            defaultQuiz = new Domain.Quiz.QuizConfiguration
            {
                DiscountPercentage = -1,
                Id = 0,
                IsActive = true,
                IsDefault = true,
                PassPercentage = -1,
                Questions = null,
                VersionNumber = 1,
            };
            _dbContext.QuizConfigurations.Add(defaultQuiz);
            await _dbContext.SaveAsync(cancellationToken);
        }

        DefaultQuizConfigResponseDto response = new DefaultQuizConfigResponseDto
        {
            MaxDiscountPercentage = defaultQuiz.DiscountPercentage >= 0 ? defaultQuiz.DiscountPercentage : null,
            PassPercentage = defaultQuiz.PassPercentage >= 0 ? defaultQuiz.PassPercentage : null,
            Questions = defaultQuiz.Questions?.Select(x => new QuizQuestionConfigResponseDto
            {
                Id = x.Id,
                Question = x.Question,
                Mark = x.Mark,
                Order = x.Order,
                QuestionImageAbsUrl = !string.IsNullOrEmpty(x.QuestionImageRelativePath) ? _fileStorage.GetObjectUrl(x.QuestionImageRelativePath) : null,
                TimeLimitInSeconds = x.TimeLimitInSeconds > 0 ? x.TimeLimitInSeconds : null,
                Answers = x.Answers?.Select(y => new QuizQuestionAnswerConfigResponseDto
                {
                    AnswerText = y.AnswerText,
                    AnswerType = y.AnswerType,
                    Id = y.Id,
                    Order = y.Order,
                    IsCorrectAnswer = y.IsCorrectAnswer,
                    AnswerImageAbsUrl = y.AnswerImage != null ? _fileStorage.GetObjectUrl(y.AnswerImage.RelativePath) : null
                })?.ToArray()
            })?.ToArray(),
            QuizId = null,
            QuizVersionNumber = defaultQuiz.VersionNumber
        };
        return response;
    }
}

