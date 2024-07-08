using Learning.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Master;

public class ContactUsRequestEFConfig : IEntityTypeConfiguration<ContactUsRequest>
{
    public void Configure(EntityTypeBuilder<ContactUsRequest> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(15);
        builder.Property(x => x.EmailAddress).IsRequired(false).HasMaxLength(200);

        builder.HasIndex(x => x.Name);

    }
}
