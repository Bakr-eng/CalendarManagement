using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalendarManagement.Infrastructure.Data.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> entity)
        {
            entity.Property(l => l.Name).IsRequired().HasMaxLength(200);
            entity.Property(l => l.City).IsRequired().HasMaxLength(100);
            entity.Property(l => l.Address).HasMaxLength(300);
        }
        
    }
    


   
}
