﻿using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamConfigurationEFConfig : IEntityTypeConfiguration<ModelExamConfiguration>
{
    public void Configure(EntityTypeBuilder<ModelExamConfiguration> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.ExamName).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(500);
        builder.Property(x => x.CreatedOn).IsRequired(true);
        builder.Property(x => x.CreatedBy).HasMaxLength(36).IsRequired(true);
        builder.Property(x => x.ExamSolutionVideoId).IsRequired(false);

        builder.HasOne(x => x.ExamSolutionVideo)
            .WithOne()
            .HasForeignKey<ModelExamConfiguration>(x => x.ExamSolutionVideoId);

        builder.HasOne(x => x.ModelExamPackage)
            .WithMany(x => x.ModelExamConfigs)
            .HasForeignKey(x => x.ModelExamPackageId);

        builder.HasOne(x => x.ExamNotification)
            .WithMany()
            .HasForeignKey(x => x.ExamNotificationId)
            .IsRequired(false);

        builder.HasMany(x => x.Questions)
            .WithOne(x => x.ExamConfig)
            .HasForeignKey(x => x.ExamConfigId);
    }
}
