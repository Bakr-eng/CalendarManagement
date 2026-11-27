using CalendarManagement.Domain.Enums;

namespace CalendarManagement.Application.DTOs;

public record UpdateCustomerRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone,
    CustomerType CustomerType = CustomerType.Company,
    string? IdentityNumber = null
    );