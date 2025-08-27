using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using ClosedXML.Excel;
using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.ViewModels;
using GoToWorkDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xceed.Words.NET;

namespace GoToWorkBusinessLogic.Implementations;

public class ReportContract(
    GoToWorkDbContext context,
    ILogger logger)
    : IReportContract
{
    public async Task<List<WorkshopsReportViewModel>> GetWorkshopsByDetailsAsync(List<string> selectedDetailIds,
        CancellationToken ct)
    {
        return await context.Workshops
            .Include(w => w.Production)
            .ThenInclude(p => p!.DetailProductions)!
            .ThenInclude(dp => dp.Detail)
            .Where(w => w.Production!.DetailProductions!
                .Any(dp => selectedDetailIds.Contains(dp.DetailId)))
            .Select(w => new WorkshopsReportViewModel
            {
                WorkshopId = w.Id,
                Address = w.Address,
                ProductionName = w.Production!.Name,
                RelatedDetails = w.Production.DetailProductions!
                    .Where(dp => selectedDetailIds.Contains(dp.DetailId))
                    .Select(dp => dp.Detail!.Name)
                    .ToList()
            })
            .ToListAsync(ct);
    }

    public async Task<Stream> CreateDocxDocumentWorkshopsByDetailsAsync(List<WorkshopsReportViewModel> data,
        CancellationToken ct)
    {
        var stream = new MemoryStream();
        using var doc = DocX.Create(stream);
        var table = doc.InsertTable(data.Count + 1, 4);

        // Заголовки
        table.Rows[0].Cells[0].Paragraphs.First().Append("Id Цеха");
        table.Rows[0].Cells[1].Paragraphs.First().Append("Адрес");
        table.Rows[0].Cells[2].Paragraphs.First().Append("Название производства");
        table.Rows[0].Cells[3].Paragraphs.First().Append("Связанные детали");

        // Данные
        for (var i = 0; i < data.Count; i++)
        {
            table.Rows[i + 1].Cells[0].Paragraphs.First().Append(data[i].WorkshopId);
            table.Rows[i + 1].Cells[1].Paragraphs.First().Append(data[i].Address);
            table.Rows[i + 1].Cells[2].Paragraphs.First().Append(data[i].ProductionName);
            table.Rows[i + 1].Cells[3].Paragraphs.First().Append(string.Join(", ", data[i].RelatedDetails));
        }

        doc.Save();
        stream.Position = 0;
        return stream;
    }

    public async Task<Stream> CreateXlsxDocumentWorkshopsByDetailsAsync(List<WorkshopsReportViewModel> data,
        CancellationToken ct)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Workshops");

        // Заголовки
        worksheet.Cell(1, 1).Value = "Id Цеха";
        worksheet.Cell(1, 2).Value = "Адрес";
        worksheet.Cell(1, 3).Value = "Название производства";
        worksheet.Cell(1, 4).Value = "Связанные детали";

        // Данные
        for (var i = 0; i < data.Count; i++)
        {
            worksheet.Cell(i + 2, 1).Value = data[i].WorkshopId;
            worksheet.Cell(i + 2, 2).Value = data[i].Address;
            worksheet.Cell(i + 2, 3).Value = data[i].ProductionName;
            worksheet.Cell(i + 2, 4).Value = string.Join(", ", data[i].RelatedDetails);
        }

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;
        return stream;
    }


    public async Task<List<DetailsReportViewModel>> GetDetailsByMachinesAndProductionsAsync(DateTime startDate,
        DateTime endDate, CancellationToken ct)
    {
        return await context.Details
            .Include(d => d.DetailProductions)!
            .ThenInclude(dp => dp.Production)
            .Include(d => d.DetailProducts)!
            .ThenInclude(dp => dp.Product)
            .ThenInclude(p => p!.Machine)
            .Where(d => d.CreationDate >= startDate && d.CreationDate <= endDate)
            .Select(d => new DetailsReportViewModel
            {
                DetailName = d.Name,
                CreationDate = d.CreationDate,
                Material = d.Material.ToString(),
                RelatedProductions = d.DetailProductions!.Select(dp => dp.Production!.Name).ToList(),
                RelatedMachines = d.DetailProducts!.Select(dp => dp.Product!.Machine!.Model).Distinct().ToList(),
                QuantityInProducts = d.DetailProducts!.Sum(dp => dp.Quantity)
            })
            .ToListAsync(ct);
    }

    public async Task<Stream> CreatePdfDocumentDetailsByMachinesAndProductionsAsync(
        List<DetailsReportViewModel> data,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct)
    {
        logger.LogInformation("CreatePdfDocumentDetailsByMachinesAndProductionsAsync: {a}, {b}, {c}", data.Count, startDate.ToString(), endDate.ToString());
        // HTML-шаблон для PDF
        var htmlContent = new StringBuilder();
        htmlContent.Append(@"
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body { font-family: Arial, sans-serif; margin: 20px; }
                        h1 { text-align: center; font-size: 24px; }
                        table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                        th, td { border: 1px solid black; padding: 8px; text-align: left; }
                        th { background-color: #f2f2f2; font-weight: bold; }
                    </style>
                </head>
                <body>
                    <h1>Отчет по деталям за период ");
        htmlContent.Append($"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
        htmlContent.Append(@"</h1>
                    <table>
                        <tr>
                            <th>Деталь</th>
                            <th>Дата создания</th>
                            <th>Материал</th>
                            <th>Производства</th>
                            <th>Станки</th>
                            <th>Количество в продуктах</th>
                        </tr>");

        // Добавление данных в таблицу
        foreach (var item in data)
        {
            htmlContent.Append("<tr>");
            htmlContent.Append($"<td>{System.Web.HttpUtility.HtmlEncode(item.DetailName)}</td>");
            htmlContent.Append($"<td>{item.CreationDate:dd.MM.yyyy}</td>");
            htmlContent.Append($"<td>{System.Web.HttpUtility.HtmlEncode(item.Material)}</td>");
            htmlContent.Append(
                $"<td>{System.Web.HttpUtility.HtmlEncode(string.Join(", ", item.RelatedProductions))}</td>");
            htmlContent.Append(
                $"<td>{System.Web.HttpUtility.HtmlEncode(string.Join(", ", item.RelatedMachines))}</td>");
            htmlContent.Append($"<td>{item.QuantityInProducts}</td>");
            htmlContent.Append("</tr>");
        }

        htmlContent.Append(@"
                    </table>
                </body>
                </html>");

        // Временные файлы
        string tempHtmlPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
        string tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf");
        var stream = new MemoryStream();

        try
        {
            // Сохраняем HTML во временный файл
            await File.WriteAllTextAsync(tempHtmlPath, htmlContent.ToString(), Encoding.UTF8, ct);

            // Запускаем wkhtmltopdf
            var processInfo = new ProcessStartInfo
            {
                FileName = "wkhtmltopdf",
                Arguments = $"--encoding UTF-8 {tempHtmlPath} {tempPdfPath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = processInfo })
            {
                process.Start();
                await process.WaitForExitAsync(ct);
                if (process.ExitCode != 0)
                {
                    string error = await process.StandardError.ReadToEndAsync();
                    throw new Exception($"Ошибка wkhtmltopdf: {error}");
                }
            }

            // Читаем PDF в поток
            byte[] pdfBytes = await File.ReadAllBytesAsync(tempPdfPath, ct);
            await stream.WriteAsync(pdfBytes, 0, pdfBytes.Length, ct);
            stream.Position = 0;
            return stream;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при создании PDF: {ex.Message}", ex);
        }
        finally
        {
            // Очистка временных файлов
            if (File.Exists(tempHtmlPath)) File.Delete(tempHtmlPath);
            if (File.Exists(tempPdfPath)) File.Delete(tempPdfPath);
        }
    }

    public async Task SendEmailAsync(Stream fileStream, string recipientEmail, string subject, string fileName,
        string contentType)
    {
        var smtpClient = new SmtpClient("smtp.yandex.ru", 465)
        {
            Credentials = new NetworkCredential("vasmaae@yandex.ru", "ofgekxnlntcomanz"),
            EnableSsl = true
        };

        var message = new MailMessage("vasmaae@yandex.ru", recipientEmail)
        {
            Subject = subject,
            Body = "Файл прикреплен.",
            IsBodyHtml = false
        };

        fileStream.Position = 0;
        message.Attachments.Add(new Attachment(fileStream, fileName, contentType));

        await smtpClient.SendMailAsync(message);
    }
}