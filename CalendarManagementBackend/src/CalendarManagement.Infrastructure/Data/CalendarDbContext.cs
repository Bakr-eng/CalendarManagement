using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Data;

public class CalendarDbContext(DbContextOptions<CalendarDbContext> options) : DbContext(options)
{
    public DbSet<Exempel> Exempel => Set<Exempel>();
    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Hämtar alla IEntityTypeConfiguration-filer automatiskt
        builder.ApplyConfigurationsFromAssembly(typeof(CalendarDbContext).Assembly);

        builder.Entity<Exempel>(entity =>
        {
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.HasData(
                new Exempel { Id = 1, Name = "Första exemplet", CreatedAt = new DateTime(2026, 11, 12) },
                new Exempel { Id = 2, Name = "Andra exemplet", CreatedAt = new DateTime(2026, 11, 12) }
            );
        });
    }
}