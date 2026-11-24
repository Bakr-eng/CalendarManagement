using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class ChecklistItemService(
    IChecklistItemRepository repository)
    : IChecklistItemService
{
    public async Task<List<ChecklistItem>> GetByActivityIdAsync(
        int activityId,
        CancellationToken ct = default)
    {
        return await repository.GetByActivityIdAsync(
            activityId,
            ct);
    }

    public async Task AddAsync(
        ChecklistItem item,
        CancellationToken ct = default)
    {
        await repository.AddAsync(item, ct);
        await repository.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken ct = default)
    {
        await repository.DeleteAsync(id, ct);
        await repository.SaveChangesAsync(ct);
    }
}