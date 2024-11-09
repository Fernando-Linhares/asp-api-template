using Api.App.Features.Mailer;

namespace Api.App.Mails;

public class ForgotPasswordMailBase: MailBase
{
    public override string Subject { get; set; } = "Forgot password";
    public string VisitorToken { get; set; } = string.Empty;
    public override string To { get; set; } = string.Empty;
    protected override string Body()
    {
        var url = ConfigApp.Get("app.frontend") + "/auth/reset-password/" + VisitorToken;
        
        return $"Click here to reset your password <a style=\"color: white; background: teal; padding: 5px; border-radius: 7px; margin: 5px;\" href=\"{url}\">Link</a>";
    }
}