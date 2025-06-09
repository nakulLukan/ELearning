using Learning.Domain.Identity;
using Learning.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class ApplicationUserOtherDetailEFConfig : IEntityTypeConfiguration<ApplicationUserOtherDetail>
{
    public void Configure(EntityTypeBuilder<ApplicationUserOtherDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PhoneNumber).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.FullName).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired(false);
        builder.Property(x => x.NormalizedEmail).IsRequired(false);

        builder.Property(x => x.YearOfBirth).IsRequired(false);
        builder
            .Property(x => x.Place)
            .HasMaxLength(DomainConstant.PlaceFieldMaxLength)
            .HasDefaultValue(string.Empty)
            .IsRequired(true);
    }
}
