using CalendarManagement.Domain.Enums;

namespace CalendarManagement.Application.DTOs;

public record CustomerDto(
    int Id,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone,
    CustomerType CustomerType,
    string? IdentityNumber
    );