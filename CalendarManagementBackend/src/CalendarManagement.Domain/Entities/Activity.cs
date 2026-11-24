

namespace CalendarManagement.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;

    public int? ResponsiblePersonId { get; set; }
    public User? ResponsiblePerson { get; set; } // Navigation property för related User entity

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; } // Navigation property för related Customer entity

    public int? LocationId { get; set; }
    public Location? Location { get; set; } // Navigation property för related Location entity

    public ICollection<ChecklistItem> ChecklistItems { get; set; }
        = new List<ChecklistItem>();
}

