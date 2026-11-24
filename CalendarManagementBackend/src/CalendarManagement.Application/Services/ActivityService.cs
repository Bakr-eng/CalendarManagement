using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class ActivityService(
    IActivityRepository repository,
    IUserRepository userRepository,
    ICustomerRepository customerRepository,
    ILocationRepository locationRepository) : IActivityService
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

    public async Task<List<ActivityDto>> GetAllAsync(ActivityFilter filter, CancellationToken ct = default)
    {
        var activities = await repository.GetAllAsync(filter, ct);
        return activities.Select(ToDto).ToList();
    }

    public async Task<ActivityDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var activity = await repository.GetByIdAsync(id, ct);
        return activity is null ? null : ToDto(activity);
    }

    public async Task<ActivityDto> CreateAsync(CreateActivityRequest request, CancellationToken ct = default)
    {
        var errors = new Dictionary<string, string[]>();

        if (!request.Date.HasValue)
        {
            errors["date"] = ["Datum måste anges."];
        }

        var activityType = AllowedTypes.FirstOrDefault(
            type => string.Equals(type, request.Type?.Trim(), StringComparison.OrdinalIgnoreCase));

        if (activityType is null)
        {
            errors["type"] = ["Ogiltig aktivitetstyp."];
        }

        if (request.ResponsiblePersonId.HasValue &&
            await userRepository.GetByIdAsync(request.ResponsiblePersonId.Value, ct) is null)
        {
            errors["responsiblePersonId"] = ["Den valda personen finns inte."];
        }

        if (request.CustomerId.HasValue &&
            await customerRepository.GetByIdAsync(request.CustomerId.Value, ct) is null)
        {
            errors["customerId"] = ["Den valda kunden finns inte."];
        }

        if (request.LocationId.HasValue &&
            await locationRepository.GetByIdAsync(request.LocationId.Value, ct) is null)
        {
            errors["locationId"] = ["Den valda platsen finns inte."];
        }

        if (errors.Count > 0)
        {
            throw new RequestValidationException(errors);
        }

        var activity = new Activity
        {
            Date = request.Date!.Value,
            Type = activityType!,
            ResponsiblePersonId = request.ResponsiblePersonId,
            CustomerId = request.CustomerId,
            LocationId = request.LocationId
        };

        await repository.AddAsync(activity, ct);

        // Hämta aktiviteten igen, med person, kund och plats, så att svaret innehåller namnen
        var saved = await repository.GetByIdAsync(activity.Id, ct);
        return ToDto(saved ?? activity);
    }

    private static ActivityDto ToDto(Activity activity) // Konverterar en Activity entity till en ActivityDto
    {
        return new ActivityDto
        {
            Id = activity.Id,
            Date = activity.Date,
            Type = activity.Type,
            ResponsiblePersonId = activity.ResponsiblePersonId,
            ResponsiblePersonName = activity.ResponsiblePerson?.Name,
            CustomerId = activity.CustomerId,
            CustomerName = activity.Customer?.Name,
            LocationId = activity.LocationId,
            LocationName = activity.Location?.Name,
            LocationCity = activity.Location?.City
        };
    }
}