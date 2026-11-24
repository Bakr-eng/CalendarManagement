namespace CalendarManagement.Domain.Entities;

public class Customer
{
    public const int NameMaxLength = 200;
    public const int ContactPersonMaxLength = 200;
    public const int EmailMaxLength = 200;
    public const int PhoneMaxLength = 50;



    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}