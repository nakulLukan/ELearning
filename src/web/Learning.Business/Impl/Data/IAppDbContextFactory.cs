namespace Learning.Business.Impl.Data;

public interface IAppDbContextFactory
{
    IAppDbContext CreateDbContext();
}
