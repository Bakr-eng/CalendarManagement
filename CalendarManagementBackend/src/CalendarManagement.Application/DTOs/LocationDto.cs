namespace CalendarManagement.Application.DTOs;

public record LocationDto(
    int Id,
    string Name,
    string City,
    string? Address
    );