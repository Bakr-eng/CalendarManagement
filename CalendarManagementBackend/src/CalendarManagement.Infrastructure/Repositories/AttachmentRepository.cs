using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class AttachmentRepository(CalendarDbContext db)
    : IAttachmentRepository
{
    public Task<List<ActivityAttachment>> GetAllAsync(
        int activityId,
        CancellationToken ct = default) =>
        db.ActivityAttachments
            .AsNoTracking()
            .Where(a => a.ActivityId == activityId)
            .OrderByDescending(a => a.UploadedAtUtc)
            .ToListAsync(ct);
    public Task<ActivityAttachment?> GetByIdAsync(
        int activityId,
        int attachmentId,
        CancellationToken ct = default) =>
        db.ActivityAttachments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.Id == attachmentId &&
                     a.ActivityId == activityId, ct);
    public async Task AddAsync(
        ActivityAttachment attachment,
        CancellationToken ct = default)
    {
        db.ActivityAttachments.Add(attachment);
        await db.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(
        ActivityAttachment attachment,
        CancellationToken ct = default)
    {
        db.ActivityAttachments.Remove(attachment);
        await db.SaveChangesAsync(ct);
    }
}
