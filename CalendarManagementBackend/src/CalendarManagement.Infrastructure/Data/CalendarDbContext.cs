using CalendarManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Data;

public class CalendarDbContext(DbContextOptions<CalendarDbContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<ChecklistItem> ChecklistItems => Set<ChecklistItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Hämtar alla IEntityTypeConfiguration-filer automatiskt
        builder.ApplyConfigurationsFromAssembly(typeof(CalendarDbContext).Assembly);
       
    }
}