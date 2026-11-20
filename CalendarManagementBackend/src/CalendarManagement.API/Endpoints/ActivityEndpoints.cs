using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.API.Endpoints;

public static class ActivityEndpoints
{
    private static readonly string[] AllowedTypes =
    {
        "Gig",
        "Private",
        "Rehearsal",
        "Vacation",
        "Travel Day",
        "Pre-production",
        "Individual Work"
    };

    public static IEndpointRouteBuilder MapActivityEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/activities")
            .WithTags("Activities");

        group.MapGet("/", async (IActivityRepository repository) =>
        {
            var activities = await repository.GetAllAsync();

            return Results.Ok(activities);
        });

        group.MapGet("/{id:int}", async (
            int id,
            IActivityRepository repository) =>
        {
            var activities = await repository.GetAllAsync();

            var activity = activities.FirstOrDefault(a => a.Id == id);

            if (activity is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(activity);
        });

        group.MapPost("/", async (
            CreateActivityDto dto,
            IActivityRepository repository,
             IUserRepository userRepository) =>
        {
            if (!dto.Date.HasValue)
            {
                return Results.BadRequest("Date is required.");
            }

            var activityType = AllowedTypes.FirstOrDefault(
                type => string.Equals(
                    type,
                    dto.Type?.Trim(),
                    StringComparison.OrdinalIgnoreCase
                )
            );

            if (activityType == null)
            {
                return Results.BadRequest("Invalid activity type.");
            }

            if (dto.ResponsiblePersonId.HasValue)
            {
                var user = await userRepository.GetByIdAsync(
                    dto.ResponsiblePersonId.Value
                );

                if (user == null)
                {
                    return Results.BadRequest(
                        "Selected responsible person does not exist."
                    );
                }
            }


            var activity = new Activity
            {
                Date = dto.Date.Value,
                Type = activityType,
                ResponsiblePersonId = dto.ResponsiblePersonId
            };

            var createdActivity =
                await repository.AddAsync(activity);

            return Results.Json(
                createdActivity,
                statusCode: StatusCodes.Status201Created
            );
        });

        return app;
    }
}