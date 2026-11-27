using CalendarManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CalendarManagement.Infrastructure.Storage;

public class LocalAttachmentStorage : IAttachmentStorage
{
    private readonly string _folder;
    public LocalAttachmentStorage(IConfiguration configuration)
    {
        var configuredPath =
            configuration["AttachmentStorage:Path"];

        _folder = string.IsNullOrWhiteSpace(configuredPath)
            ? Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "CalendarManagement",
                "attachments")
            : Path.GetFullPath(configuredPath);

        Directory.CreateDirectory(_folder);
    }
    private string GetPath(string storedFileName)
    {
        if (!Guid.TryParseExact(storedFileName, "N", out _))
        {
            throw new InvalidOperationException(
                "Ogiltigt lagringsnamn.");
        }

        return Path.Combine(_folder, storedFileName);
    }
    public async Task SaveAsync(
        string storedFileName,
        Stream content,
        CancellationToken ct = default)
    {
        var path = GetPath(storedFileName);

        await using var output = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            FileOptions.Asynchronous);

        await content.CopyToAsync(output, ct);
    }
    public Stream OpenRead(string storedFileName)
    {
        return new FileStream(
            GetPath(storedFileName),
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            81920,
            FileOptions.Asynchronous);
    }
    public Task DeleteAsync(
        string storedFileName,
        CancellationToken ct = default)
    {
        var path = GetPath(storedFileName);

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }
}
