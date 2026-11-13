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

    public async Task<List<Activity>> GetAllAsync()
    {
        return await _context.Activities
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Activity> AddAsync(Activity activity)
    {
        _context.Activities.Add(activity);

        await _context.SaveChangesAsync();

        return activity;
    }
}

