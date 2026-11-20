using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct = default);
    Task<User> AddAsync(User user, CancellationToken ct = default);
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
}
