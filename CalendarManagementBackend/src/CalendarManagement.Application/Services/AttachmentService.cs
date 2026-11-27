
using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class AttachmentService(
    IActivityRepository activityRepository,
    IAttachmentRepository repository,
    IAttachmentStorage storage) : IAttachmentService
{
    private const long MaxFileSize = 100L * 1024 * 1024;

    private static readonly Dictionary<string, string> AllowedTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = "application/pdf",
            [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            [".pptx"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            [".txt"] = "text/plain",
            [".csv"] = "text/csv",
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".webp"] = "image/webp",
            [".gif"] = "image/gif"
        };

    private static AttachmentDto ToDto(ActivityAttachment a) =>
        new(a.Id, a.ActivityId, a.FileName,
            a.ContentType, a.FileSize, a.UploadedAtUtc);

    private static void Invalid(string message)
    {
        throw new RequestValidationException(
            new Dictionary<string, string[]>
            {
                ["file"] = [message]
            });
    }
    public async Task<List<AttachmentDto>> GetAllAsync(
        int activityId, CancellationToken ct = default)
    {
        var files = await repository.GetAllAsync(activityId, ct);
        return files.Select(ToDto).ToList();
    }
    public async Task<AttachmentDto?> GetByIdAsync(
        int activityId, int attachmentId,
        CancellationToken ct = default)
    {
        var attachment = await repository.GetByIdAsync(
            activityId, attachmentId, ct);

        return attachment is null ? null : ToDto(attachment);
    }
    public async Task<AttachmentDto?> UploadAsync(
        int activityId,
        string fileName,
        long fileSize,
        Stream content,
        CancellationToken ct = default)
    {
        if (await activityRepository.GetByIdAsync(
                activityId, ct) is null)
        {
            return null;
        }

        var safeName = Path.GetFileName(
            fileName.Replace('\\', '/')).Trim();

        if (string.IsNullOrWhiteSpace(safeName) ||
            safeName.Length > 260)
        {
            Invalid("Ogiltigt filnamn.");
        }

        var extension = Path.GetExtension(safeName);

        if (!AllowedTypes.TryGetValue(extension, out var contentType))
        {
            Invalid("Filtypen stöds inte.");
        }

        if (fileSize <= 0 || fileSize > MaxFileSize)
        {
            Invalid("Filen måste vara mellan 1 byte och 100 MB.");
        }

        var storedFileName = Guid.NewGuid().ToString("N");

        var attachment = new ActivityAttachment
        {
            ActivityId = activityId,
            FileName = safeName,
            StoredFileName = storedFileName,
            ContentType = contentType!,
            FileSize = fileSize,
            UploadedAtUtc = DateTime.UtcNow
        };

        try
        {
            await storage.SaveAsync(storedFileName, content, ct);
            await repository.AddAsync(attachment, ct);
        }
        catch
        {
            // Rensa eventuell fil om uppladdningen misslyckas.
            try
            {
                await storage.DeleteAsync(
                    storedFileName, CancellationToken.None);
            }
            catch
            {
                // Rensningsfel bör loggas i produktion.
            }

            throw;
        }

        return ToDto(attachment);
    }
    public async Task<AttachmentDownload?> DownloadAsync(
        int activityId,
        int attachmentId,
        CancellationToken ct = default)
    {
        var attachment = await repository.GetByIdAsync(
            activityId, attachmentId, ct);

        if (attachment is null)
        {
            return null;
        }

        return new AttachmentDownload(
            storage.OpenRead(attachment.StoredFileName),
            attachment.FileName,
            attachment.ContentType);
    }
    public async Task<bool> DeleteAsync(
        int activityId,
        int attachmentId,
        CancellationToken ct = default)
    {
        var attachment = await repository.GetByIdAsync(
            activityId, attachmentId, ct);

        if (attachment is null)
        {
            return false;
        }

        await repository.DeleteAsync(attachment, ct);
        await storage.DeleteAsync(
            attachment.StoredFileName, ct);

        return true;
    }
}
