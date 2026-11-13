using CalendarManagement.Application.DTOs;

namespace CalendarManagement.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync(CancellationToken ct = default);
    Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);
}