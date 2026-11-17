using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;

namespace CalendarManagement.API.Endpoints
{
    public static class LocationEndpoints
    {
        public static void MapLocationEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/locations").WithTags("Locations");

            group.MapGet("/", async (ILocationService service, CancellationToken ct) =>
                Results.Ok(await service.GetAllAsync(ct)));

            group.MapGet("/{id:int}", async (int id, ILocationService service, CancellationToken ct) =>
            {
                var location = await service.GetByIdAsync(id, ct);
                return location is null ? Results.NotFound() : Results.Ok(location);
            }).WithName("GetLocationById");

            group.MapPost("/", async (CreateLocationDto request, ILocationService service, CancellationToken ct) =>
            {
                var errors = new Dictionary<string, string[]>();

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    errors["name"] = ["Namn måste fyllas i."];
                }

                if (string.IsNullOrWhiteSpace(request.City))
                {
                    errors["city"] = ["Stad måste fyllas i."];
                }

                if (errors.Count > 0)
                {
                    return Results.ValidationProblem(errors);
                }

                var created = await service.CreateAsync(request, ct);
                return Results.CreatedAtRoute("GetLocationById", new { id = created.Id }, created);
            });
        }
    }
}