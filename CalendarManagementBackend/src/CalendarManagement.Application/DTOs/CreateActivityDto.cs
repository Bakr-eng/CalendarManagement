using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CalendarManagement.Application.DTOs;

public class CreateActivityDto
{
    [Required]
    public DateTime? Date { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;
}
public class ActivityDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
}