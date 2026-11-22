using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class UserRepository(CalendarDbContext db) : IUserRepository
{
    public async Task<List<User>> GetAllAsync(
        CancellationToken ct = default)
    {
        return await db.Users
            .AsNoTracking()
            .OrderBy(user => user.Name)
            .ToListAsync(ct);
    }

    public async Task<User> AddAsync(User user, CancellationToken ct = default)
    {
        db.Users.Add(user);

        await db.SaveChangesAsync(ct);

        return user;
    }
    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await db.Users.FindAsync([id], ct);
    }
}
