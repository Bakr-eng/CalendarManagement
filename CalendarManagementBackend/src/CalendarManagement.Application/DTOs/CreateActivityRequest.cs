using System.ComponentModel.DataAnnotations;

namespace CalendarManagement.Application.DTOs;

public class CreateActivityRequest
{
    [Required(ErrorMessage = "Datum måste anges.")]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Aktivitetstyp måste anges.")]
    public string Type { get; set; } = string.Empty;
    public int? ResponsiblePersonId { get; set; }
}