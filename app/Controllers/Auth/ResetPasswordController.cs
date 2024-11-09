using Api.App.Controllers.Requests.Auth;
using Api.App.Features.Jwt;
using Api.Database;
using Api.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles="account.reset.password")]
[Route("/auth/reset-password")]
public class ResetPasswordController(DatabaseContext context, IStateLessSession stateLessSession) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
    {
        var user = await LoggedUser();
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        context.Users.Update(user);
        await context.SaveChangesAsync();

        var rules = JsonConvert.DeserializeObject<string[]>(user.Rules);

        if (rules == null)
        {
            return AnswerInternalError("Failed to parse rules");
        }
        
        var session = stateLessSession.CreateUserSession(user.Id, user.Email, user.Name, rules);
        
        return AnswerSuccess(session);
    }
}