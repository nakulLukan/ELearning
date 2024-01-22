using Microsoft.EntityFrameworkCore;

namespace Learning.Infrastructure.Data.Seeder.MigrationSeeder;

public static class SeederBase
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        LookupSeeder.SeedData(modelBuilder);
    }
}
