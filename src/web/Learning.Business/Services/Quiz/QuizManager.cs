using Learning.Business.Contracts.Persistence;
using Learning.Business.Impl.Data;
using Learning.Shared.Application.Contracts.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Business.Services.Quiz;

public class QuizManager
{
    private readonly IAppCache _appCache;
    private readonly IAppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;

    public QuizManager(IAppCache appCache, IAppDbContext dbContext, IFileStorage fileStorage)
    {
        _appCache = appCache;
        _dbContext = dbContext;
        _fileStorage = fileStorage;
    }

}
