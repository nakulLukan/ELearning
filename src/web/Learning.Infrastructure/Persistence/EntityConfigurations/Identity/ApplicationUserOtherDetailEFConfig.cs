using Learning.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class ApplicationUserOtherDetailEFConfig : IEntityTypeConfiguration<ApplicationUserOtherDetail>
{
    public void Configure(EntityTypeBuilder<ApplicationUserOtherDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PhoneNumber).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(30);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}
