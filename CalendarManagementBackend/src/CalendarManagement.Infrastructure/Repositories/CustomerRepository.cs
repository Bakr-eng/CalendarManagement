using CalendarManagement.Application.Customers;
using CalendarManagement.Domain.Entities;
using CalendarManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarManagement.Infrastructure.Repositories;

public class CustomerRepository(CalendarDbContext db) : ICustomerRepository
{
    public Task<List<Customer>> GetAllAsync(CancellationToken ct = default) =>
        db.Customers
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);

    public Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default) =>
        db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        db.Customers.Add(customer);
        await db.SaveChangesAsync(ct);
    }
}
