using Learning.Business.Impl.Data;

namespace Learning.Business.Contracts.Services.Master;

public interface IAppMasterSettingManager
{
    public Task<T?> GetValue<T>(IAppDbContext dbContext, string key);
    public Task SetValue<T>(IAppDbContext dbContext, string key, T value, CancellationToken cancellationToken);
}
