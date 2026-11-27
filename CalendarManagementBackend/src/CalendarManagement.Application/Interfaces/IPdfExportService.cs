using CalendarManagement.Application.DTOs;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Interfaces;

public interface IPdfExportService
{
    byte[] GenerateActivityPdf(
        ActivityDto activity,
        List<ChecklistItem> checklistItems);
}
