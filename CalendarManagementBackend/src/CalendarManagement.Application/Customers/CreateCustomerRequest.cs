namespace CalendarManagement.Application.Customers;

public record CreateCustomerRequest(
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
    );