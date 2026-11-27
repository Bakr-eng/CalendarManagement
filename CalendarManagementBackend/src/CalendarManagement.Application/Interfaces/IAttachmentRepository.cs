using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IAttachmentRepository
{
    Task<List<ActivityAttachment>> GetAllAsync(
        int activityId,
        CancellationToken ct = default);
    Task<ActivityAttachment?> GetByIdAsync(
        int activityId,
        int attachmentId,
        CancellationToken ct = default);
    Task AddAsync(
        ActivityAttachment attachment,
        CancellationToken ct = default);
    Task DeleteAsync(
        ActivityAttachment attachment,
        CancellationToken ct = default);
}
