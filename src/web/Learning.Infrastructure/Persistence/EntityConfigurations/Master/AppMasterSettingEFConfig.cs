using Learning.Domain.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class AppMasterSettingEFCOnfig : IEntityTypeConfiguration<AppMasterSetting>
{
    public void Configure(EntityTypeBuilder<AppMasterSetting> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(50);
        builder.Property(x => x.Value).IsRequired(false).HasMaxLength(1000);
    }
}
