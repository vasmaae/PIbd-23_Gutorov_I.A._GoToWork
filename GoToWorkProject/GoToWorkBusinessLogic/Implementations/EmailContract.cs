using GoToWorkContracts.BusinessLogicContracts;
using GoToWorkContracts.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace GoToWorkBusinessLogic.Implementations;

public class EmailContract(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
    : IEmailContract
{
    public static EmailContract CreateYandexService()
    {
        return new EmailContract(
            smtpServer: "smtp.yandex.ru",
            smtpPort: 465,
            smtpUsername: "vasmaae@yandex.ru",
            smtpPassword: "ofgekxnlntcomanz"
        );
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Завод «Иди работать»", smtpUsername));
        email.To.Add(new MailboxAddress("Работник", toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        if (attachmentPath is not null && !attachmentPath.IsEmpty())
            await builder.Attachments.AddAsync(attachmentPath);

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpServer, smtpPort);
        await smtp.AuthenticateAsync(smtpUsername, smtpPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendTestEmailAsync(string toEmail)
    {
        await SendEmailAsync(
            toEmail: toEmail,
            subject: "Завод зовёт!!!",
            body: "<h1>Тестовое письмо</h1>" +
                  "<p>Проверка работы системы распространения отчётности.</p>"
        );
    }
}