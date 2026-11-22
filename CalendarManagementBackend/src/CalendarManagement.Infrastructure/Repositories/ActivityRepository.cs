using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;
public class ActivityRepository(CalendarDbContext db) : IActivityRepository
{
    public async Task<List<Activity>> GetAllAsync(
        CancellationToken ct = default)
    {
        return await db.Activities
            .Include(a => a.ResponsiblePerson)
            .AsNoTracking()
            .ToListAsync(ct);
    }
    public async Task<Activity?> GetByIdAsync(int id,
        CancellationToken ct = default)
    {
        return await db.Activities
            .Include(a => a.ResponsiblePerson)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<Activity> AddAsync(
        Activity activity,
        CancellationToken ct = default)
    {
        db.Activities.Add(activity);

        await db.SaveChangesAsync(ct);

        return activity;
    }
}

