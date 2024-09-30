using Learning.Domain.Master;
using Learning.Shared.Common.Constants;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Data.Seeder.MigrationSeeder;

public class AppSettingSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        List<AppMasterSetting> settings = new List<AppMasterSetting>()
        {
            new()
            {
                Id = AppMasterSettingKeyConstant.COG_USER_MIN_DATE,
                IsJsonValue = true,
                Value = null
            },
            new()
            {
                Id = AppMasterSettingKeyConstant.COG_USER_LIST_PAGINATION,
                IsJsonValue = true,
                Value = null
            }
        };

        foreach (var setting in settings)
        {
            modelBuilder.Entity<AppMasterSetting>().HasData(setting);
        }
    }
}
