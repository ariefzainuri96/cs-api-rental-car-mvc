using ClosedXML.Excel;
using System.IO;
using System.Threading.Tasks;
using cs_api_rental_car_mvc.Data;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace cs_api_rental_car_mvc.Services.ReportService
{
    public class ReportService : IReportService
    {

        private readonly RentalCarDbContext context;

        public ReportService(RentalCarDbContext context)
        {
            this.context = context;
        }

        public async Task<byte[]> GenerateExcelReport()
        {
            var rents = await context.Rents
            .Include(r => r.Car)
            .Include(r => r.User)
            .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Rents");

            // Header
            worksheet.Cell(1, 1).Value = "User";
            worksheet.Cell(1, 2).Value = "Car";
            worksheet.Cell(1, 3).Value = "Start Date";
            worksheet.Cell(1, 4).Value = "End Date";
            worksheet.Cell(1, 5).Value = "Actual End Date";
            worksheet.Cell(1, 6).Value = "Total Amount";

            // Rows
            int row = 2;
            foreach (var rent in rents)
            {
                worksheet.Cell(row, 1).Value = rent.User?.Name;
                worksheet.Cell(row, 2).Value = $"{rent.Car?.Brand} - {rent.Car?.Model}";
                worksheet.Cell(row, 3).Value = rent.StartDate;
                worksheet.Cell(row, 4).Value = rent.EndDate;
                worksheet.Cell(row, 5).Value = rent.ActualEndDate;
                worksheet.Cell(row, 6).Value = rent.TotalAmount;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> GeneratePdfReport()
        {
            var rents = await context.Rents
            .Include(r => r.Car)
            .Include(r => r.User)
            .ToListAsync();

            // Create document
            using var document = new PdfDocument();

            // Landscape page
            var page = document.AddPage();
            page.Size = PdfSharpCore.PageSize.A4;
            page.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12, XFontStyle.Regular);

            int yPoint = 40;

            // Header
            gfx.DrawString("Rental Report", new XFont("Arial", 18, XFontStyle.Bold), XBrushes.Black,
                new XRect(0, yPoint, page.Width, 30), XStringFormats.TopCenter);
            yPoint += 40;

            // Table header
            int x = 20;
            int[] columnWidths = { 80, 100, 80, 80, 100, 80 }; // Widths for User, Car, Start, End, Actual End, Total

            string[] headers = { "User", "Car", "Start Date", "End Date", "Actual End Date", "Total" };
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawString(headers[i], font, XBrushes.Black, new XRect(x, yPoint, columnWidths[i], 20), XStringFormats.TopLeft);
                x += columnWidths[i];
            }
            yPoint += 25;

            // Table rows
            foreach (var rent in rents)
            {
                x = 20; // reset x to start
                gfx.DrawString(rent.User?.Name ?? "-", font, XBrushes.Black, new XRect(x, yPoint, columnWidths[0], 20), XStringFormats.TopLeft); x += columnWidths[0];
                gfx.DrawString($"{rent.Car?.Brand} - {rent.Car?.Model}", font, XBrushes.Black, new XRect(x, yPoint, columnWidths[1], 20), XStringFormats.TopLeft); x += columnWidths[1];
                gfx.DrawString(rent.StartDate.ToString("yyyy-MM-dd"), font, XBrushes.Black, new XRect(x, yPoint, columnWidths[2], 20), XStringFormats.TopLeft); x += columnWidths[2];
                gfx.DrawString(rent.EndDate.ToString("yyyy-MM-dd"), font, XBrushes.Black, new XRect(x, yPoint, columnWidths[3], 20), XStringFormats.TopLeft); x += columnWidths[3];
                gfx.DrawString(rent.ActualEndDate?.ToString("yyyy-MM-dd") ?? "-", font, XBrushes.Black, new XRect(x, yPoint, columnWidths[4], 20), XStringFormats.TopLeft); x += columnWidths[4];
                gfx.DrawString(rent.TotalAmount.ToString("C"), font, XBrushes.Black, new XRect(x, yPoint, columnWidths[5], 20), XStringFormats.TopLeft);

                yPoint += 25;

                // Add new page if exceeded page height
                if (yPoint > page.Height - 50)
                {
                    page = document.AddPage();
                    page.Size = PdfSharpCore.PageSize.A4;
                    page.Orientation = PdfSharpCore.PageOrientation.Landscape;
                    gfx = XGraphics.FromPdfPage(page);
                    yPoint = 40;
                }
            }

            using var stream = new MemoryStream();
            document.Save(stream, false);
            return stream.ToArray();


        }
    }
}
