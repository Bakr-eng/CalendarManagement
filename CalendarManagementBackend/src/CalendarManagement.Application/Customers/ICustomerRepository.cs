using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Customers;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken ct = default);
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
}