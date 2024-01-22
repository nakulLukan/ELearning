using Learning.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Core;

public class ClassDivisionEFConfig : IEntityTypeConfiguration<ClassDivision>
{
    public void Configure(EntityTypeBuilder<ClassDivision> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(15);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(50);

        builder.HasMany(x => x.Subjects)
            .WithOne(x => x.Class)
            .HasForeignKey(x => x.ClassId);
    }
}
