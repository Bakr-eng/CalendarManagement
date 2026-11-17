using CalendarManagement.Application.DTOs;
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
            var location = new Location
            {
                Name = request.Name.Trim(),
                City = request.City.Trim(),
                Address = request.Address?.Trim()
            };

            await repository.AddAsync(location, ct);
            return ToDto(location);
        }

        private static LocationDto ToDto(Location l) =>
            new(l.Id, l.Name, l.City, l.Address);
    }
}