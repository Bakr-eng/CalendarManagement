using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;

namespace CalendarManagement.API.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customers").WithTags("Customers");

            group.MapGet("/", async (ICustomerService service, CancellationToken ct) =>
                Results.Ok(await service.GetAllAsync(ct)));

            group.MapGet("/{id:int}", async (int id, ICustomerService service, CancellationToken ct) =>
            {
                var customer = await service.GetByIdAsync(id, ct);
                return customer is null ? Results.NotFound() : Results.Ok(customer);
            }).WithName("GetCustomerById");

            group.MapPost("/", async (CreateCustomerRequest request, ICustomerService service, CancellationToken ct) =>
            {
                var created = await service.CreateAsync(request, ct);
                return Results.CreatedAtRoute("GetCustomerById", new { id = created.Id }, created);
            });

            group.MapPut("/{id:int}", async (int id, UpdateCustomerRequest request, ICustomerService service, CancellationToken ct) =>
            {
                var updated = await service.UpdateAsync(id, request, ct);
                return updated is null ? Results.NotFound() : Results.Ok(updated);
            });

            group.MapDelete("/{id:int}", async (int id, ICustomerService service, CancellationToken ct) =>
            {
                var deleted = await service.DeleteAsync(id, ct);
                return deleted ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}