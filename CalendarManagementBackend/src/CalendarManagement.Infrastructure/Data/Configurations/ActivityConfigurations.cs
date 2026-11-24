using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarManagement.Infrastructure.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Type)
            .IsRequired()
            .HasMaxLength(50);

        // Tas en kund bort blir CustomerId null på aktiviteterna – aktiviteten finns kvar
        builder.HasOne(a => a.Customer)
            .WithMany()
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Tas en plats bort blir LocationId null på aktiviteterna – aktiviteten finns kvar
        builder.HasOne(a => a.Location)
            .WithMany()
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Snabbare när kalendern hämtar och sorterar på datum
        builder.HasIndex(a => a.Date);
    }
}