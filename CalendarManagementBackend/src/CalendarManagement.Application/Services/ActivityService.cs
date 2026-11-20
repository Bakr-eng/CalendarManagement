
using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class ActivityService(
    IActivityRepository repository,
    IUserRepository userRepository) : IActivityService
{
    private static readonly string[] AllowedTypes =
    [
        "Gig",
        "Private",
        "Rehearsal",
        "Vacation",
        "Travel Day",
        "Pre-production",
        "Individual Work"
    ];

    public async Task<List<ActivityDto>> GetAllAsync(
        CancellationToken ct = default)
    {
        var activities = await repository.GetAllAsync(ct);

        return activities.Select(ToDto).ToList();
    }

    public async Task<ActivityDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        var activity = await repository.GetByIdAsync(id, ct);

        return activity is null ? null : ToDto(activity);
    }

    public async Task<ActivityDto> CreateAsync(
        CreateActivityDto request,
        CancellationToken ct = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (!request.Date.HasValue)
        {
            errors["date"] = ["Date is required."];
        }

        var activityType = AllowedTypes.FirstOrDefault(
            type => string.Equals(
                type,
                request.Type?.Trim(),
                StringComparison.OrdinalIgnoreCase)
        );

        if (activityType is null)
        {
            errors["type"] = ["Invalid activity type."];
        }

        if (request.ResponsiblePersonId.HasValue)
        {
            var user = await userRepository.GetByIdAsync(
                request.ResponsiblePersonId.Value, ct);

            if (user is null)
            {
                errors["responsiblePersonId"] =
                    ["Selected user does not exist."];
            }
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }

        var activity = new Activity
        {
            Date = request.Date!.Value,
            Type = activityType!,
            ResponsiblePersonId = request.ResponsiblePersonId
        };

        await repository.AddAsync(activity, ct);

        // Hämta personen för DTO-svaret
        if (activity.ResponsiblePersonId.HasValue)
        {
            activity.ResponsiblePerson =
                await userRepository.GetByIdAsync(
                    activity.ResponsiblePersonId.Value, ct);
        }

        return ToDto(activity);
    }

    private static ActivityDto ToDto(Activity activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            Date = activity.Date,
            Type = activity.Type,
            ResponsiblePersonId =
                activity.ResponsiblePersonId,
            ResponsiblePersonName =
                activity.ResponsiblePerson?.Name
        };
    }
}
