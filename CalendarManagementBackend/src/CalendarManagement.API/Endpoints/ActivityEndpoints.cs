using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;

namespace CalendarManagement.API.Endpoints;

public static class ActivityEndpoints
{
    public static IEndpointRouteBuilder MapActivityEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/activities")
            .WithTags("Activities");

        group.MapGet("/", async (
            [AsParameters] ActivityFilter filter,
            IActivityService service,
            CancellationToken ct) =>
        {
            var activities = await service.GetAllAsync(filter, ct);

            return Results.Ok(activities);
        });

        group.MapGet("/{id:int}", async (
            int id,
            IActivityService service,
            CancellationToken ct) =>
        {
            var activity = await service.GetByIdAsync(id, ct);

            return activity is null
                ? Results.NotFound()
                : Results.Ok(activity);
        })
        .WithName("GetActivityById");

        group.MapGet("/{id:int}/pdf", async (
            int id,
            IActivityService activityService,
            IPdfExportService pdfService,
            CancellationToken ct) =>
        {
            var activity = await activityService.GetByIdAsync(id, ct);

            if (activity is null)
            {
                return Results.NotFound();
            }

            var pdf = pdfService.GenerateActivityPdf(activity);

            return Results.File(
                pdf,
                "application/pdf",
                $"activity-{id}.pdf");
        });

        group.MapPost("/", async (
            CreateActivityRequest request,
            IActivityService service,
            CancellationToken ct) =>
        {
            var created = await service.CreateAsync(request, ct);

            return Results.CreatedAtRoute(
                "GetActivityById",
                new { id = created.Id },
                created);
        });

        return app;
    }
}