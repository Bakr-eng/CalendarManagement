namespace CalendarManagement.Application.DTOs;

public class CreateChecklistItemRequest
{
    public int ActivityId { get; set; }

    public string Section { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}