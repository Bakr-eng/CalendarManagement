namespace CalendarManagement.Application.DTOs;

public class ChecklistItemDto
{
    public int Id { get; set; }

    public int ActivityId { get; set; }

    public string Section { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public bool Completed { get; set; }
}