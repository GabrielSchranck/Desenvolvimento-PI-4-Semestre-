namespace BookAPI.Services.Email
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(IEnumerable<string> emailsTo, string subject, string body, IEnumerable<string> attachments);
    }
}
