namespace CalendarManagement.Application.Interfaces;

public interface IAttachmentStorage
{
    Task SaveAsync(
        string storedFileName,
        Stream content,
        CancellationToken ct = default);
    Stream OpenRead(string storedFileName);
    Task DeleteAsync(
        string storedFileName,
        CancellationToken ct = default);
}
