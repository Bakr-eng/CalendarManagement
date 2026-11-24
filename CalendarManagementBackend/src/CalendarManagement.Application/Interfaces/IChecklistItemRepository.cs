using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IChecklistItemRepository
{
    Task<List<ChecklistItem>> GetByActivityIdAsync(
        int activityId,
        CancellationToken ct = default);

    Task AddAsync(
        ChecklistItem item,
        CancellationToken ct = default);

    Task DeleteAsync(
        int id,
        CancellationToken ct = default);

    Task SaveChangesAsync(
        CancellationToken ct = default);

    Task UpdateCompletedAsync(
    int id,
    bool completed,
    CancellationToken ct = default);
}