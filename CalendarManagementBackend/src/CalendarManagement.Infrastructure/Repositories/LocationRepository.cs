using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace CalendarManagement.Infrastructure.Repositories
{
    public class LocationRepository(CalendarDbContext db) : ILocationRepository
    {
        public async Task<List<Location>> GetAllAsync(CancellationToken ct = default)
        {
            return await db.Locations
                .AsNoTracking()
                .OrderBy(l => l.Name)
                .ThenBy(l => l.City) // det är bra att sortera på stad
                .ToListAsync(ct);
        }

        public async Task<Location?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await db.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id, ct);
        }

        public async Task AddAsync(Location location, CancellationToken ct = default)
        {
            db.Locations.Add(location);
            await db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Location location, CancellationToken ct = default)
        {
            db.Locations.Update(location);
            await db.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var deletedRows = await db.Locations
                .Where(l => l.Id == id)
                .ExecuteDeleteAsync(ct);

            return deletedRows > 0;
        }
    }
}
