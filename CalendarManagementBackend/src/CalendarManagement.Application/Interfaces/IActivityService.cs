using CalendarManagement.Application.DTOs;

namespace CalendarManagement.Application.Interfaces;

public interface IActivityService
{
    Task<List<ActivityDto>> GetAllAsync(
        CancellationToken ct = default);

    Task<ActivityDto?> GetByIdAsync(
        int id,
        CancellationToken ct = default);

    Task<ActivityDto> CreateAsync(
        CreateActivityRequest request,
        CancellationToken ct = default);
}
