using CalendarManagement.Application.DTOs;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IActivityRepository
{
    Task<List<Activity>> GetAllAsync(
        ActivityFilter filter,
        CancellationToken ct = default);

    Task<Activity?> GetByIdAsync(
        int id,
        CancellationToken ct = default);

    Task<Activity> AddAsync(
        Activity activity,
        CancellationToken ct = default);
}