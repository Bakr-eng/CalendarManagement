using CalendarManagement.Application.DTOs;

public interface IPdfExportService
{
    byte[] GenerateActivityPdf(ActivityDto activity);
}