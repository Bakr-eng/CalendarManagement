namespace CalendarManagement.Application.Customers;

public record CustomerDto(
    int Id,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
    );