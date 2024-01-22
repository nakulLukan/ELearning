using Learning.Domain.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class LookupValueEFConfig : IEntityTypeConfiguration<LookupValue>
{
    public void Configure(EntityTypeBuilder<LookupValue> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.InternalName).IsRequired(false).HasMaxLength(20);
        builder.Property(x => x.DisplayValue).IsRequired().HasMaxLength(100);

        builder.HasOne(x => x.LookupMaster)
            .WithMany(x => x.LookupValues)
            .HasForeignKey(x => x.LookupMasterId);
    }
}
