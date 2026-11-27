using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarManagement.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.Property(c => c.Name).IsRequired().HasMaxLength(Customer.NameMaxLength);
        entity.Property(c => c.ContactPerson).HasMaxLength(Customer.ContactPersonMaxLength);
        entity.Property(c => c.Email).HasMaxLength(Customer.EmailMaxLength);
        entity.Property(c => c.Phone).HasMaxLength(Customer.PhoneMaxLength);

        // Enum sparas som int. Befintliga kunder får 0 = Company för att inte bryta databasen just nu
        entity.Property(c => c.CustomerType).IsRequired();
        entity.Property(c => c.IdentityNumber).HasMaxLength(Customer.IdentityNumberMaxLength);

        entity.HasIndex(c => c.Name); // snabbare sökning på kundnamn senare
    }
}