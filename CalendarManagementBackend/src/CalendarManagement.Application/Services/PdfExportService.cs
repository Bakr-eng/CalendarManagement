using CalendarManagement.Application.DTOs;
using CalendarManagement.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using CalendarManagement.Domain.Entities;

namespace CalendarManagement.Application.Services;

public class PdfExportService : IPdfExportService
{
    public byte[] GenerateActivityPdf(
    ActivityDto activity,
    List<ChecklistItem> checklistItems)
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
                    .PaddingTop(20)
                    .Column(column =>
                    {
                        column.Item()
                            .Text("Activity Details")
                            .FontSize(16)
                            .Bold();

                        column.Item().PaddingTop(10);

                        column.Item().Text($"Activity ID: {activity.Id}");

                        column.Item().Text(
                            $"Type: {activity.Type}");

                        column.Item().Text(
                            $"Date: {activity.Date:yyyy-MM-dd}");

                        column.Item().Text(
                            $"Responsible Person: {activity.ResponsiblePersonName ?? "Not assigned"}");

                        column.Item().Text(
                            $"Customer: {activity.CustomerName ?? "No customer"}");

                        var locationText =
                            activity.LocationName ?? "No location";

                        if (!string.IsNullOrWhiteSpace(activity.LocationCity))
                        {
                            locationText += $", {activity.LocationCity}";
                        }

                        column.Item().Text(
                            $"Location: {locationText}");
                            column.Item().PaddingTop(20);

column.Item()
    .Text("Checklist")
    .FontSize(16)
    .Bold();

foreach (var item in checklistItems)
{
    var marker = item.Completed ? "[X]" : "[ ]";

    column.Item().Text(
        $"{item.Section} - {marker} {item.Text}");
}
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Generated ");
                        text.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        }).GeneratePdf();
    }
}