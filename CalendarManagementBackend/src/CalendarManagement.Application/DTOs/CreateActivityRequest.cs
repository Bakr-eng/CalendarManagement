namespace CalendarManagement.Application.DTOs;

public class CreateActivityRequest
{
    public DateTime? Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? ResponsiblePersonId { get; set; }
    public int? CustomerId { get; set; }
    public int? LocationId { get; set; }
}