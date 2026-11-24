

using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllAsync(CancellationToken ct = default);
        Task<Location?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(Location location, CancellationToken ct = default);
        Task UpdateAsync(Location location, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
