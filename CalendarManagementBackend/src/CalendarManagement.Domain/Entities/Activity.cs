

namespace CalendarManagement.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? ResponsiblePersonId { get; set; }
    public User? ResponsiblePerson { get; set; }
}

