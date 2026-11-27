
namespace CalendarManagement.Domain.Entities;

public class ActivityAttachment
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public DateTime UploadedAtUtc { get; set; }
}
