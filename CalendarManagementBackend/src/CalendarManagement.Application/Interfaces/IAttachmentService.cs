using CalendarManagement.Application.DTOs;

namespace CalendarManagement.Application.Interfaces
{
    public interface IAttachmentService
    {
        Task<List<AttachmentDto>> GetAllAsync(
            int activityId, CancellationToken ct = default);
        Task<AttachmentDto?> GetByIdAsync(
            int activityId, int attachmentId,
            CancellationToken ct = default);
        Task<AttachmentDto?> UploadAsync(
            int activityId, string fileName,
            long fileSize, Stream content,
            CancellationToken ct = default);
        Task<AttachmentDownload?> DownloadAsync(
            int activityId, int attachmentId,
            CancellationToken ct = default);
        Task<bool> DeleteAsync(
            int activityId, int attachmentId,
            CancellationToken ct = default);
    }
}
