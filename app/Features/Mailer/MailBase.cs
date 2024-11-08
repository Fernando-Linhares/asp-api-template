using System.Net.Mail;
using Api.App.Features.Queue;

namespace Api.App.Features.Mailer;

public abstract class MailBase: IOnQueue
{
    public abstract string Subject { get; set; }
    public abstract string To { get; set; }
    protected abstract string Body();
    public virtual List<string>? Attachments { get; init; }
    protected virtual string Template {get;set;} = "default";
    
    public MailMessage ToMessage()
    {
        return new MailMessage
        {
            Subject = Subject,
            From = new MailAddress(GetRootSmtpEmail()),
            Body =  Body(),
            IsBodyHtml = true,
        };
    }

    private string GetRootSmtpEmail()
    {
        return ConfigApp.Get("smtp.email");
    }

    public OutputDispatcher DispatchOnQueue()
    {
        var dispatcher = new QueueDispatcher();
        return dispatcher.Dispatch("mail", this);
    }

    private async Task<EmailDispatchOutput> SendAsync()
    {
        IEmailDispatcher dispatcher = new EmailDispatcher();
        return await dispatcher.SendAsync(this);
    }

    public async Task Handle()
    {
        Console.WriteLine(Subject);
        await SendAsync();
    }

    public object Serialize()
    {
        return new
        {
            To = To,
            Subject = Subject,
            Body = RenderTemplate(Subject, Body()),
            Attachments = Attachments
        };
    }

    private string RenderTemplate(string title, string body)
    {
        return File.ReadAllText(
                Path.Combine(Directory.GetCurrentDirectory(),
                "templates",
                Template + ".html"
            )
        )
        .Replace("@title@", title)
        .Replace("@body@", body);
    }
}