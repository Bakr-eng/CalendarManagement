namespace CalendarManagement.Application.DTOs;

public record CreateLocationRequest(
    string Name,
    string City,
    string? Address
    );
