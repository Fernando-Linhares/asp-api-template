using Api.App.Features.Mailer;

namespace Api.App.Mails;

public class AccountConfirmationMailBase: MailBase
{
    public string Code { get; set; } = string.Empty;
    public override string Subject { get; set; } = "Account confirmation";
    public override string To { get; set; } = string.Empty;
    protected override string Body()
    {
        int len = Code.Length;
        string nCode = "";

        for(var i = 0; i< len; i++) {

            if(i != 0) {
                nCode += "-";
            }

            nCode += Code[i];
        }
        
        return $"Your confirmation code is <span style=\"margin: 5px; padding: 5px; border:solid white 2px; border-radius: 7px;font-size-22;\">{nCode}</span>.";
    }
}