namespace CalendarManagement.Domain.Entities;

public class ChecklistItem
{
    public int Id { get; set; }

    public int ActivityId { get; set; }

    public string Section { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public bool Completed { get; set; }

    public Activity Activity { get; set; } = null!;
}