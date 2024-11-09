using Api.App.Mails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "account.email.code")]
[Route("/auth/send-email-confirmation")]
public class SendEmailConfirmationController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> SendEmail()
    {
        var user = await LoggedUser();

        var confirmationCodeEmail = new AccountConfirmationMailBase
        {
            Code = user.ConfirmationCode,
            To = user.Email
        };
        
        return AnswerSuccess(confirmationCodeEmail.DispatchOnQueue());
    }
}