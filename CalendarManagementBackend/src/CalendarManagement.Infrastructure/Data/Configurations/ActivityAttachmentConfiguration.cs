using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarManagement.Infrastructure.Configurations;

public class ActivityAttachmentConfiguration
    : IEntityTypeConfiguration<ActivityAttachment>
{
    public void Configure(
        EntityTypeBuilder<ActivityAttachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(260);

        builder.Property(a => a.StoredFileName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.ContentType)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.FileSize)
            .IsRequired();

        builder.Property(a => a.UploadedAtUtc)
            .IsRequired();

        builder.HasOne<Activity>()
            .WithMany()
            .HasForeignKey(a => a.ActivityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
