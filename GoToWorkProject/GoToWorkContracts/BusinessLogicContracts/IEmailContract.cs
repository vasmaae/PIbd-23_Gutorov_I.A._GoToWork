namespace GoToWorkContracts.BusinessLogicContracts;

public interface IEmailContract
{
    public Task SendEmailAsync(string to, string subject, string body, string? attachmentPath = null);
}