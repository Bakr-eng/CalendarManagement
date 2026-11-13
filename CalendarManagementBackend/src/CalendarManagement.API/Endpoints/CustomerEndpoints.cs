using CalendarManagement.Application.Customers;

namespace CalendarManagement.API.Endpoints;

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
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>
                {
                    ["name"] = ["Namn måste fyllas i."]
                });
            }

            var created = await service.CreateAsync(request, ct);
            return Results.CreatedAtRoute("GetCustomerById", new { id = created.Id }, created);
        });
    }
}