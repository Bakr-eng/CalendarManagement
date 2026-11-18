using CalendarManagement.Application.DTOs;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces
{
    public interface ILocationService
    {
     Task<List<LocationDto>> GetAllAsync(CancellationToken ct = default);

     Task<LocationDto?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<LocationDto> CreateAsync(CreateLocationRequest request, CancellationToken ct = default);


    }
}
