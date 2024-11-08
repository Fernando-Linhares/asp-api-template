using Api.App.Features.Mailer;
using Newtonsoft.Json;

namespace Api.App.Features.Queue.Handlers;

public class EmailQueueHandler : IQueueHandler
{
    public async Task Execute(string wrapJson)
    {
         var mail = JsonConvert.DeserializeObject<dynamic>(wrapJson);
         if (mail is not null)
         {
             var instance =  new Mail
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