using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
}
