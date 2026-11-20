using System;
using System.Collections.Generic;
using System.Text;

namespace CalendarManagement.Application.DTOs
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
        public int? ResponsiblePersonId { get; set; }
        public string? ResponsiblePersonName { get; set; }
    }
}
