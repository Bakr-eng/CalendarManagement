using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarManagement.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
        entity.Property(c => c.ContactPerson).HasMaxLength(200);
        entity.Property(c => c.Email).HasMaxLength(200);
        entity.Property(c => c.Phone).HasMaxLength(50);

        entity.HasIndex(c => c.Name); // snabbare sökning på kundnamn senare
    }
}