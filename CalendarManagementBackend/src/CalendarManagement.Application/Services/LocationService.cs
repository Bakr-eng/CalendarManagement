using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Exceptions;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services
{
    public class LocationService(ILocationRepository repository) : ILocationService
    {
        public async Task<List<LocationDto>> GetAllAsync(CancellationToken ct = default)
        {
            var locations = await repository.GetAllAsync(ct);
            return locations.Select(ToDto).ToList();
        }

        public async Task<LocationDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var location = await repository.GetByIdAsync(id, ct);
            return location is null ? null : ToDto(location);
        }

        public async Task<LocationDto> CreateAsync(CreateLocationRequest request, CancellationToken ct = default)
        {
            Validate(request.Name, request.City, request.Address);

            var location = new Location
            {
                Name = request.Name.Trim(),
                City = request.City.Trim(),
                Address = request.Address?.Trim()
            };

            await repository.AddAsync(location, ct);
            return ToDto(location);
        }

        public async Task<LocationDto?> UpdateAsync(int id, UpdateLocationRequest request, CancellationToken ct = default)
        {
            Validate(request.Name, request.City, request.Address);

            var location = await repository.GetByIdAsync(id, ct);
            if (location is null)
            {
                return null;
            }

            location.Name = request.Name.Trim();
            location.City = request.City.Trim();
            location.Address = request.Address?.Trim();

            await repository.UpdateAsync(location, ct);
            return ToDto(location);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken ct = default) =>
            repository.DeleteAsync(id, ct);

        private static void Validate(string? name, string? city, string? address)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors["name"] = ["Namn måste fyllas i."];
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                errors["city"] = ["Stad måste fyllas i."];
            }

            CheckMaxLength(errors, "name", name, Location.NameMaxLength, "Namn");
            CheckMaxLength(errors, "city", city, Location.CityMaxLength, "Stad");
            CheckMaxLength(errors, "address", address, Location.AddressMaxLength, "Adress");

            if (errors.Count > 0)
            {
                throw new RequestValidationException(errors);
            }
        }

        private static void CheckMaxLength(
            Dictionary<string, string[]> errors, string field, string? value, int maxLength, string label)
        {
            if (value is not null && value.Trim().Length > maxLength)
            {
                errors[field] = [$"{label} får vara högst {maxLength} tecken."];
            }
        }

        private static LocationDto ToDto(Location l) =>
            new(l.Id, l.Name, l.City, l.Address);
    }
}