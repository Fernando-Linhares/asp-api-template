namespace Api.App.Features.Mailer;

public class Mail: MailBase
{
    public override string Subject { get; set; }
    public override string To { get; set; }
    public string BodyText { get; set; }
    protected override string Body()
    {
        return BodyText;
    }
}