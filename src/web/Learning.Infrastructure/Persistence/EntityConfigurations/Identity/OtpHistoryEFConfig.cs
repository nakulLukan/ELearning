using Learning.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class OtpHistoryEFConfig : IEntityTypeConfiguration<OtpHistory>
{
    public void Configure(EntityTypeBuilder<OtpHistory> builder)
    {
        builder.HasIndex(x => x.UserName);
        builder.Property(x => x.Id);
        builder.Property(x => x.UserName).IsRequired().HasMaxLength(10);
    }
}
