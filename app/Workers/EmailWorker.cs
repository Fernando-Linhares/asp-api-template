using Api.App.Features.Mailer;
using Api.App.Features.PubSub;

namespace Api.App.Workers;

public class EmailWorker: IWorker
{
    public Message? Message { get; set; }
    public async Task Execute()
    {
        var mail = Message?.Deserialize<dynamic>();
        if (mail is not null)
        {
            var instance = new Mail
            {
                To = mail.To,
                Subject = mail.Subject,
                BodyText = mail.Body,
                Attachments = mail.Attachments
            };
            await instance.Handle();
        }
    }
}