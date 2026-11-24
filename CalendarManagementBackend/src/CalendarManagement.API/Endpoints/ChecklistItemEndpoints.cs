using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.API.Endpoints;

public static class ChecklistItemEndpoints
{
    public static void MapChecklistItemEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/checklists")
            .WithTags("Checklist");

        group.MapGet(
            "/activity/{activityId:int}",
            async (
                int activityId,
                IChecklistItemService service,
                CancellationToken ct) =>
            {
                var items = await service.GetByActivityIdAsync(
                    activityId,
                    ct);

                return Results.Ok(items);
            });

        group.MapPost("/", async (
            CreateChecklistItemRequest request,
            IChecklistItemService service,
            CancellationToken ct) =>
        {
            var item = new ChecklistItem
            {
                ActivityId = request.ActivityId,
                Section = request.Section,
                Text = request.Text,
                Completed = false
            };

            await service.AddAsync(item, ct);

            return Results.Ok(new
            {
                item.Id,
                item.ActivityId,
                item.Section,
                item.Text,
                item.Completed
            });
        });

        group.MapDelete("/{id:int}", async (
            int id,
            IChecklistItemService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);

            return Results.NoContent();
        });

        group.MapPut("/{id:int}/completed", async (
            int id,
            bool completed,
            IChecklistItemService service,
            CancellationToken ct) =>
        {
            await service.UpdateCompletedAsync(id, completed, ct);

            return Results.Ok();
        });
        
    }
}