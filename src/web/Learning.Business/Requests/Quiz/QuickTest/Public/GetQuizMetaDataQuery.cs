using Learning.Business.Impl.Data;
using Learning.Shared.Dto.Quiz.QuickTest.Public;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Requests.Quiz.QuickTest.Public;

public class GetQuizMetaDataQuery : IRequest<QuizMetaDataDto>
{
}

public class GetQuizMetaDataQueryHandler : IRequestHandler<GetQuizMetaDataQuery, QuizMetaDataDto>
{
    private readonly IAppDbContext _dbContext;

    public GetQuizMetaDataQueryHandler(
        IAppDbContextFactory dbContext)
    {
        _dbContext = dbContext.CreateDbContext();
    }

    public async Task<QuizMetaDataDto> Handle(GetQuizMetaDataQuery request, CancellationToken cancellationToken)
    {
        var defaultQuiz = await _dbContext.QuizConfigurations
            .Where(x => x.IsDefault)
            .Select(x => new QuizMetaDataDto
            {
                QuizConfigId = x.Id,
                TotalQuestions = x.Questions.Count,
                TotalTimeInSeconds = x.Questions.Sum(x => x.TimeLimitInSeconds),
                TotalMarks = x.Questions.Sum(x => x.Mark),
                MinimumPassPercentage = x.PassPercentage,
                TotalDiscount = x.DiscountPercentage,
                QuizVersionNumber = x.VersionNumber
            })
            .FirstAsync(cancellationToken);

        return defaultQuiz;
    }
}

