using Learning.Domain.Master;
using Learning.Shared.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Data.Seeder.MigrationSeeder;

public class LookupSeeder
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        List<LookupMaster> lookupMasters = new List<LookupMaster>()
        {
        new()
        {
            Id = (int)LookupMasterEnum.SubjectGroup,
            InternalName = "SubjectGroup",
            IsActive = true,
            DisplayValue = "Subject Group Names",
        },new()
        {
            Id = (int)LookupMasterEnum.ChapterGroup,
            InternalName = "ChapterGroup",
            IsActive = true,
            DisplayValue = "Chapter Group Names",
        },
    };

        foreach (var lookup in lookupMasters)
        {
            modelBuilder.Entity<LookupMaster>().HasData(lookup);
        }

        int lookupDetailId = 1;
        // Subject Group Names
        List<LookupValue> lookupDetails = SeedSubjectGroupNames(ref lookupDetailId);

        foreach (var lookupDetail in lookupDetails)
        {
            modelBuilder.Entity<LookupValue>().HasData(lookupDetail);
        }
    }

    private static List<LookupValue> SeedSubjectGroupNames(ref int lookupDetailId)
    {
        return new()
        {
            new()
            {
                Id = lookupDetailId++,
                InternalName = "language",
                IsActive = true,
                Order = 1,
                DisplayValue = "Languages",
                LookupMasterId = (int)LookupMasterEnum.SubjectGroup
            },
        };
    }
}
