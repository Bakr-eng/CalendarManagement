using CalendarManagement.Domain.Enums;

namespace CalendarManagement.Domain.Entities;

public class Customer
{
    public const int NameMaxLength = 200;
    public const int ContactPersonMaxLength = 200;
    public const int EmailMaxLength = 200;
    public const int PhoneMaxLength = 50;
    public const int IdentityNumberMaxLength = 13; // "ÅÅÅÅMMDD-XXXX" = 13 tecken

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; } = CustomerType.Company;
    public string? IdentityNumber { get; set; }

    public string? ContactPerson { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}