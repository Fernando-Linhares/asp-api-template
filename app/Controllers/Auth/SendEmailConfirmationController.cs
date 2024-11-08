using Api.App.Mails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "personal")]
[Route("/auth/send-email-confirmation")]
public class SendEmailConfirmationController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> SendEmail()
    {
        var user = await LoggedUser();

        var confirmationCodeEmail = new AccountConfirmationMailBase(user.ConfirmationCode)
        {
            To = user.Email
        };
        
        return AnswerSuccess(confirmationCodeEmail.DispatchOnQueue());
    }
}