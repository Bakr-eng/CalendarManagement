using CalendarManagement.Domain.Enums;

namespace CalendarManagement.Application.DTOs;

public record CreateCustomerRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone,
    CustomerType CustomerType = CustomerType.Company,
    string? IdentityNumber = null
    );