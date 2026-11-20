using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CalendarManagement.Application.DTOs;

public class CreateActivityDto
{
    [Required(ErrorMessage = "Date is required.")]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Activity type is required.")]
    public string Type { get; set; } = string.Empty;
    public int? ResponsiblePersonId { get; set; }
}
public class ActivityDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
}