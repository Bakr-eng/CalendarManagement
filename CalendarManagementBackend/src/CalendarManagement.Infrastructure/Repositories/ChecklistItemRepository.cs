using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class ChecklistItemRepository : IChecklistItemRepository
{
    private readonly CalendarDbContext _context;

    public ChecklistItemRepository(CalendarDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChecklistItem>> GetByActivityIdAsync(
        int activityId,
        CancellationToken ct = default)
    {
        return await _context.ChecklistItems
            .Where(x => x.ActivityId == activityId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(
        ChecklistItem item,
        CancellationToken ct = default)
    {
        await _context.ChecklistItems.AddAsync(item, ct);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken ct = default)
    {
        var item = await _context.ChecklistItems
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (item is not null)
        {
            _context.ChecklistItems.Remove(item);
        }
    }
    public async Task UpdateCompletedAsync(
    int id,
    bool completed,
    CancellationToken ct = default)
{
    var item = await _context.ChecklistItems
        .FirstOrDefaultAsync(x => x.Id == id, ct);

    if (item is not null)
    {
        item.Completed = completed;
    }
}

    public async Task SaveChangesAsync(
        CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}