namespace CalendarManagement.Application.DTOs;

public record UpdateLocationRequest(
    string Name,
    string City,
    string? Address
    );