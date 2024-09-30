using Learning.Business.Contracts.Services.Master;
using Learning.Business.Impl.Data;
using Learning.Shared.Common.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Learning.Business.Services.Master;

public class AppMasterSettingManager : IAppMasterSettingManager
{
    public async Task<T?> GetValue<T>(IAppDbContext dbContext, string key)
    {
        var setting = await dbContext.AppSettings.FirstOrDefaultAsync(x => x.Id == key);
        if (setting == null)
        {
            throw new AppException("AppMasterSettingManager: Unknown key");
        }
        if (setting.Value == null)
        {
            return default;
        }
        return System.Text.Json.JsonSerializer.Deserialize<T>(setting.Value);
    }

    public async Task SetValue<T>(IAppDbContext dbContext, string key, T value, CancellationToken cancellationToken)
    {
        var setting = await dbContext.AppSettings.AsTracking().FirstOrDefaultAsync(x => x.Id == key);
        if (setting == null)
        {
            throw new AppException("AppMasterSettingManager: Unknown key");
        }

        setting.Value = System.Text.Json.JsonSerializer.Serialize(value);
        setting.LastUpdatedOn = AppDateTime.UtcNow;

        await dbContext.SaveAsync(cancellationToken);
    }
}
