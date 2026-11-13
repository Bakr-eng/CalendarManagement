namespace CalendarManagement.Application.DTOs;

public record CreateCustomerRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
    );