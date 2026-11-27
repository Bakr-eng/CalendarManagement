using CalendarManagement.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CalendarManagement.Application.Services;

public class PdfExportService : IPdfExportService
{
    public byte[] GenerateTestPdf()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header()
                    .Text("Calendar Management")
                    .FontSize(20)
                    .Bold();

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Item().Text("Hello PDF!");
                        column.Item().Text("QuestPDF is working.");
                        column.Item().Text($"Generated: {DateTime.Now}");
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        }).GeneratePdf();
    }
}