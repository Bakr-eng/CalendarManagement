using CalendarManagement.Application.Interfaces;

namespace CalendarManagement.API.Endpoints
{
    public static class AttachmentEndpoints
    {
        public static IEndpointRouteBuilder MapAttachmentEndpoints(
            this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(
                "/api/activities/{activityId:int}/attachments")
                .WithTags("Attachments");

            group.MapGet("/", async (
                int activityId,
                IAttachmentService service,
                CancellationToken ct) =>
            {
                var files = await service.GetAllAsync(activityId, ct);
                return Results.Ok(files);
            });

            group.MapGet("/{attachmentId:int}", async (
                int activityId,
                int attachmentId,
                IAttachmentService service,
                CancellationToken ct) =>
            {
                var file = await service.GetByIdAsync(
                    activityId, attachmentId, ct);

                return file is null
                    ? Results.NotFound()
                    : Results.Ok(file);
            });

            group.MapPost("/", async (
                int activityId,
                IFormFile file,
                IAttachmentService service,
                CancellationToken ct) =>
            {
                await using var stream = file.OpenReadStream();

                var created = await service.UploadAsync(
                    activityId,
                    file.FileName,
                    file.Length,
                    stream,
                    ct);

                return created is null
                    ? Results.NotFound()
                    : Results.Created(
                        $"/api/activities/{activityId}/attachments/{created.Id}",
                        created);
            });

            group.MapGet("/{attachmentId:int}/download", async (
                int activityId,
                int attachmentId,
                IAttachmentService service,
                CancellationToken ct) =>
            {
                var download = await service.DownloadAsync(
                    activityId, attachmentId, ct);

                return download is null
                    ? Results.NotFound()
                    : Results.File(
                        download.Content,
                        contentType: "application/octet-stream",
                        fileDownloadName: download.FileName);
            });

            group.MapDelete("/{attachmentId:int}", async (
                int activityId,
                int attachmentId,
                IAttachmentService service,
                CancellationToken ct) =>
            {
                var deleted = await service.DeleteAsync(
                    activityId, attachmentId, ct);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            });

            group.MapGet("/{attachmentId:int}/preview", async (
                int activityId,
                int attachmentId,
                IAttachmentService service,
                HttpContext context,
                CancellationToken ct) =>
            {
                var file = await service.GetByIdAsync(
                    activityId, attachmentId, ct);

                if (file is null)
                    return Results.NotFound();

                string[] previewTypes =
                [
                    "image/jpeg",
                    "image/png",
                    "image/webp",
                    "image/gif",
                    "application/pdf"
                ];

                if (!previewTypes.Contains(file.ContentType))
                    return Results.BadRequest(
                        "Den här filtypen kan inte förhandsgranskas.");

                var download = await service.DownloadAsync(
                    activityId, attachmentId, ct);

                if (download is null)
                    return Results.NotFound();

                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers.CacheControl = "private, no-store";

                return Results.File(
                    download.Content,
                    contentType: download.ContentType,
                    enableRangeProcessing: true);
            });

            return app;
        }
    }
}
