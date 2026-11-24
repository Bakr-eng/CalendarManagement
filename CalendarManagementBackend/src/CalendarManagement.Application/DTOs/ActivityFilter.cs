namespace CalendarManagement.Application.DTOs;

// Filter för GET /api/activities?search=...&customerId=...
// Alla är frivilliga – utan filter hämtas alla aktiviteter
public record ActivityFilter(
    string? Search,
    int? CustomerId,
    int? LocationId,
    int? ResponsiblePersonId);