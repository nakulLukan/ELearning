using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Video;

public class VideoEFConfig : IEntityTypeConfiguration<Learning.Domain.Content.Video>
{
    public void Configure(EntityTypeBuilder<Domain.Content.Video> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).IsRequired(false).HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.RelativeUrl).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.MpdFileName).IsRequired(true).HasMaxLength(50);
    }
}
