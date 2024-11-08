using System.Net;
using System.Net.Mail;

namespace Api.App.Features.Mailer;

public class EmailDispatcher: IEmailDispatcher
{
    private readonly SmtpClient _smtpClient = new(ConfigApp.Get("smtp.host"), Int32.Parse(ConfigApp.Get("smtp.port")))
    {
        Credentials = new NetworkCredential( ConfigApp.Get("smtp.username"), ConfigApp.Get("smtp.password")),
        EnableSsl = false
    };

    public async Task<EmailDispatchOutput> SendAsync(MailBase mailBase)
    {
        var instance = mailBase.ToMessage();
        instance.To.Add(mailBase.To);
        if (mailBase.Attachments != null)
        {
            foreach (string path in mailBase.Attachments)
            {
                instance.Attachments.Add(new Attachment(path));
            }
        }
        await _smtpClient.SendMailAsync(instance);

        return new EmailDispatchOutput(mailBase.To, mailBase.Subject, GetTimestamp());
    }

    private long GetTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}