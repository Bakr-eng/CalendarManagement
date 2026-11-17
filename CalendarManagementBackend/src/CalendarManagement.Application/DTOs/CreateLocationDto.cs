
namespace CalendarManagement.Application.DTOs;

public record CreateLocationDto(
    string Name,
    string City,
    string? Address
    );
