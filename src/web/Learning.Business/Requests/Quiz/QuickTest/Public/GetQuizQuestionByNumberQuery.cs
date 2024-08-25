using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using Learning.Shared.Dto.Quiz.QuickTest.Public;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Quiz.QuickTest.Public;

public class GetQuizQuestionByNumberQuery : IRequest<QuizQuestionDto>
{
    public required int QuizConfigId { get; set; }
    public required int QuestionNumber { get; set; }
}

public class GetQuizQuestionByNumberQueryHandler : IRequestHandler<GetQuizQuestionByNumberQuery, QuizQuestionDto>
{
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public GetQuizQuestionByNumberQueryHandler(
        IAppDbContextFactory dbContext,
        IFileStorage fileStorage)
    {
        _dbContext = dbContext.CreateDbContext();
        _fileStorage = fileStorage;
    }

    public async Task<QuizQuestionDto> Handle(GetQuizQuestionByNumberQuery request, CancellationToken cancellationToken)
    {
        var question = await _dbContext.QuizQuestions
            .Include(x => x.Answers)
                .ThenInclude(x => x.AnswerImage)
            .Where(x => x.QuizConfigurationId == request.QuizConfigId
                && x.Order == request.QuestionNumber)
            .Select(x => new QuizQuestionDto
            {
                Mark = x.Mark,
                Question = x.Question,
                QuestionId = x.Id,
                QuestionImageAbsUrl = !string.IsNullOrEmpty(x.QuestionImageRelativePath) ? _fileStorage.GetObjectUrl(x.QuestionImageRelativePath) : null,
                TimeLimitInSeconds = x.TimeLimitInSeconds,
                Options = x.Answers.OrderBy(y => y.Order).Select(y => new QuizOptionDto()
                {
                    IsCorrectOption = y.IsCorrectAnswer,
                    OptionId = y.Id,
                    OptionImageAbsUrl = y.AnswerImage != null ? _fileStorage.GetObjectUrl(y.AnswerImage.RelativePath) : null,
                    OptionText = y.AnswerText,
                    Order = y.Order
                }).ToArray(),
            })
            .FirstAsync(cancellationToken);

        return question;
    }
}

