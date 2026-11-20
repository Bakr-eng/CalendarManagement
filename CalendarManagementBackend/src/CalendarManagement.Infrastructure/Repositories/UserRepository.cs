using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CalendarDbContext _context;

    public UserRepository(CalendarDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(user => user.Name)
            .ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}
