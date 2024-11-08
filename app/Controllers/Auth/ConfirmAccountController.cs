using Api.App.Controllers.Requests.Auth;
using Api.App.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "personal")]
[Route("/auth/confirm-account")]
public class ConfirmAccountController(DatabaseContext context): BaseController
{
    [HttpPost]
    public async Task<IActionResult> ConfirmAccount([FromBody] ConfirmationCode confirmationCode)
    {
        var user = await LoggedUser();

        if (user.ConfirmationCode != confirmationCode.Code)
        {
            return AnswerUnauthorized(new { errorCode = "invalid_grant", code = confirmationCode.Code } );
        }
        
        user.Confirmed = true;
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return AnswerSuccess(new
        {
            UserId = user.Id, confirmationCode.Code
        });
    }
}