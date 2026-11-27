
namespace CalendarManagement.Application.DTOs;

public record AttachmentDto(
    int Id,
    int ActivityId,
    string FileName,
    string ContentType,
    long FileSize,
    DateTime UploadedAtUtc);
public record AttachmentDownload(
    Stream Content,
    string FileName,
    string ContentType);
