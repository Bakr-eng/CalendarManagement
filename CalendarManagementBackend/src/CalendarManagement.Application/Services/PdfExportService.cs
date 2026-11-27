using CalendarManagement.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CalendarManagement.Application.DTOs;

namespace CalendarManagement.Application.Services;

public class PdfExportService : IPdfExportService
{
    public byte[] GenerateActivityPdf(ActivityDto activity)
{
    return Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header()
                .Text("Activity Sheet")
                .FontSize(20)
                .Bold();

            page.Content()
                .Column(column =>
                {
                    column.Item().Text($"Activity #{activity.Id}");
                    column.Item().Text($"Type: {activity.Type}");
                    column.Item().Text($"Date: {activity.Date:yyyy-MM-dd}");
                    column.Item().Text($"Responsible: {activity.ResponsiblePersonName}");
                    column.Item().Text($"Customer: {activity.CustomerName}");
                    column.Item().Text(
                        $"Location: {activity.LocationName}, {activity.LocationCity}");
                });
        });
    }).GeneratePdf();
}
}