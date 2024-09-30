using Learning.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class ApplicationUserOtherDetailEFConfig : IEntityTypeConfiguration<ApplicationUserOtherDetail>
{
    public void Configure(EntityTypeBuilder<ApplicationUserOtherDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PhoneNumber).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.FirstName).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.Email).IsRequired(false);
        builder.Property(x => x.NormalizedEmail).IsRequired(false);

        builder.Property(x => x.YearOfBirth).IsRequired(false);
    }
}
