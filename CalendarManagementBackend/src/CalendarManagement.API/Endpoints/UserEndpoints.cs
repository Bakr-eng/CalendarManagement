using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.API.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users");

        // GET /api/users
        group.MapGet("/", async (IUserRepository repository) =>
        {
            var users = await repository.GetAllAsync();

            return Results.Ok(users.Select(user => new
            {
                user.Id,
                user.Name
            }));
        });

        // POST /api/users
        group.MapPost("/", async (
            CreateUserRequest request,
            IUserRepository repository) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            var user = new User
            {
                Name = request.Name.Trim()
            };

            var createdUser = await repository.AddAsync(user);

            return Results.Json(
                new { createdUser.Id, createdUser.Name },
                statusCode: StatusCodes.Status201Created
            );
        });

        return app;
    }
}

public record CreateUserRequest(string Name);
