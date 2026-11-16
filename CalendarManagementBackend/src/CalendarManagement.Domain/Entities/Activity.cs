using System;
using System.Collections.Generic;
using System.Text;


namespace CalendarManagement.Domain.Entities;

public class Activity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
}

