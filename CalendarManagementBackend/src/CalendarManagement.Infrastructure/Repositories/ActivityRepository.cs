using System;
using System.Collections.Generic;
using System.Text;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;
public class ActivityRepository : IActivityRepository
{
    private readonly CalendarDbContext _context;

    public ActivityRepository(CalendarDbContext context)
    {
        _context = context;
    }

    public async Task<List<Activity>> GetAllAsync(
        CancellationToken ct = default)
    {
        return await _context.Activities
            .Include(a => a.ResponsiblePerson)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Activity?> GetByIdAsync(int id,
        CancellationToken ct = default)
    {
        return await _context.Activities
            .Include(a => a.ResponsiblePerson)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<Activity> AddAsync(
        Activity activity,
        CancellationToken ct = default)
    {
        _context.Activities.Add(activity);

        await _context.SaveChangesAsync();

        return activity;
    }
}

