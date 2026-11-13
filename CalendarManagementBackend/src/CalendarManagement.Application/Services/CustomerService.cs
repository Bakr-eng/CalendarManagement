using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<List<CustomerDto>> GetAllAsync(CancellationToken ct = default)
    {
        var customers = await repository.GetAllAsync(ct);
        return customers.Select(ToDto).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var customer = await repository.GetByIdAsync(id, ct);
        return customer is null ? null : ToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
    {
        var customer = new Customer
        {
            Name = request.Name.Trim(),
            ContactPerson = request.ContactPerson?.Trim(),
            Email = request.Email?.Trim(),
            Phone = request.Phone?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(customer, ct);
        return ToDto(customer);
    }

    private static CustomerDto ToDto(Customer c) =>
        new(c.Id, c.Name, c.ContactPerson, c.Email, c.Phone); 
}