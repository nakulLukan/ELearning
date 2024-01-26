using Learning.Business.Impl.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
namespace Learning.Infrasture.Persistence;

public class ApplicationDbContextFactory : IAppDbContextFactory
{
    readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public IAppDbContext CreateDbContext()
    {
        return _dbContextFactory.CreateDbContext();
    }
}
