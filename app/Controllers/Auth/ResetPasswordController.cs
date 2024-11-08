using Api.App.Controllers.Requests.Auth;
using Api.App.Database;
using Api.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles="forgot_password")]
[Route("/auth/reset-password")]
public class ResetPasswordController(DatabaseContext context) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
    {
        var user = await LoggedUser();
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        context.Users.Update(user);
        await context.SaveChangesAsync();

        var token = Token.GenerateNewToken(user);
        context.Tokens.Update(token);
        await context.SaveChangesAsync();

        return AnswerSuccess(new
        {
            id = token.Id,
            token = token.AccessToken,
            expiresAt = token.ExpiredAt
        });
    }
}