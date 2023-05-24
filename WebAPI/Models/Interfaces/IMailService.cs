using WebAPI.Models.Email;

namespace WebAPI.Models.Interfaces;

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}
