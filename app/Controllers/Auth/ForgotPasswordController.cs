using Api.App.Controllers.Requests.Auth;
using Api.App.Database;
using Api.App.Mails;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.App.Controllers.Auth;

[ApiController]
[Route("/auth/forgot-password")]
public class ForgotPasswordController(DatabaseContext context) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ForgotPassword request)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(request.Email));

        if (user == null)
        {
            return AnswerNotFound(new { email = request.Email });
        }

        var token = Token.GenerateForgotPasswordToken(user);
        var mail = new ForgotPasswordMailBase(token.AccessToken)
        {
            To = user.Email
        };

        var queueData =  mail.DispatchOnQueue();
        
        return AnswerSuccess(new
        {
            email = mail.To,
            timestamp = queueData.Timestamp
        });
    }
}