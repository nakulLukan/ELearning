using Learning.Domain.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class LookupMasterEFConfig : IEntityTypeConfiguration<LookupMaster>
{
    public void Configure(EntityTypeBuilder<LookupMaster> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.InternalName).IsRequired().HasMaxLength(15);
        builder.Property(x => x.DisplayValue).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.LookupValues)
            .WithOne(x => x.LookupMaster)
            .HasForeignKey(x => x.LookupMasterId);
    }
}
