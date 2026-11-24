

namespace CalendarManagement.Application.DTOs
{
    public class ActivityDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;

        public int? ResponsiblePersonId { get; set; }
        public string? ResponsiblePersonName { get; set; }

        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public int? LocationId { get; set; }
        public string? LocationName { get; set; }
        public string? LocationCity { get; set; } 
    }
}
