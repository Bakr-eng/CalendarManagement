namespace CalendarManagement.Application.DTOs;

public record CustomerDto(
    int Id,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Phone
    );