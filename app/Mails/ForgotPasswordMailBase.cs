using Api.App.Features.Mailer;

namespace Api.App.Mails;

public class ForgotPasswordMailBase(string visitorToken): MailBase
{
    public override string Subject { get; set; } = "Forgot password";
    public override string To { get; set; } = string.Empty;
    protected override string Body()
    {
        var url = ConfigApp.Get("app.frontend") + "/auth/reset-password/" + visitorToken;
        
        return $"Click here to reset your password <a style=\"color: white; background: teal; padding: 5px; border-radius: 7px; margin: 5px;\" href=\"{url}\">Link</a>";
    }
}