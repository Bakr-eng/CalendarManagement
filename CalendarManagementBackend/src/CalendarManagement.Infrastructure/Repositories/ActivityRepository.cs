using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class ActivityRepository(CalendarDbContext db) : IActivityRepository
{
    public async Task<List<Activity>> GetAllAsync(ActivityFilter filter, CancellationToken ct = default)
    {
        IQueryable<Activity> query = db.Activities
            .Include(a => a.ResponsiblePerson)
            .Include(a => a.Customer)
            .Include(a => a.Location)
            .AsNoTracking();

        if (filter.CustomerId.HasValue)
        {
            query = query.Where(a => a.CustomerId == filter.CustomerId);
        }

        if (filter.LocationId.HasValue)
        {
            query = query.Where(a => a.LocationId == filter.LocationId);
        }

        if (filter.ResponsiblePersonId.HasValue)
        {
            query = query.Where(a => a.ResponsiblePersonId == filter.ResponsiblePersonId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search)) // Om sökfältet inte är tomt, filtrera på sökord
        {
            var search = filter.Search.Trim();

            query = query.Where(a =>
                a.Type.Contains(search) ||
                (a.Customer != null && a.Customer.Name.Contains(search)) ||
                (a.Location != null && (a.Location.Name.Contains(search) || a.Location.City.Contains(search))) ||
                (a.ResponsiblePerson != null && a.ResponsiblePerson.Name.Contains(search)));
        }

        return await query
            .OrderBy(a => a.Date)
            .ToListAsync(ct);
    }

    public async Task<Activity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await db.Activities
            .Include(a => a.ResponsiblePerson)
            .Include(a => a.Customer)
            .Include(a => a.Location)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<Activity> AddAsync(Activity activity, CancellationToken ct = default)
    {
        db.Activities.Add(activity);
        await db.SaveChangesAsync(ct);
        return activity;
    }
}