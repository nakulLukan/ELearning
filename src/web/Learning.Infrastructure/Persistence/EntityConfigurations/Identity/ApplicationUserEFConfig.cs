using Learning.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class ApplicationUserEFConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.Index).ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Index);
        builder.HasIndex(x => x.IsAdmin);
        builder.Property(x => x.AccountCreatedOn).IsRequired();
    }
}
