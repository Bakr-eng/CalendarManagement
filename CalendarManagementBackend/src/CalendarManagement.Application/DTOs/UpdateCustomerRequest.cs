namespace CalendarManagement.Application.DTOs;

public record UpdateCustomerRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
    );